#include <stdio.h>
#include <string.h>
#include <pthread.h>
#include <stdlib.h>
#include <unistd.h>

int counter = 0;
pthread_mutex_t lock = PTHREAD_MUTEX_INITIALIZER;

void *doSomeThing(void *arg)
{
    pthread_mutex_lock(&lock);

    for (int i = 0; i < (int)(intptr_t)arg; i++)
        counter++;

    pthread_mutex_unlock(&lock);
}

int main(int argc, char **argv)
{
    if (argc < 3)
    {
        perror("Too low arguments!");
        exit(EXIT_FAILURE);
    }

    int NUMT = atoi(argv[1]);
    pthread_t tid[NUMT];

    if (pthread_mutex_init(&lock, NULL) != 0)
    {
        perror("Mutex init failed!");
        exit(EXIT_FAILURE);
    }

    int err;

    for (int i = 0; i < NUMT; i++)
    {
        err = pthread_create(&(tid[i]), NULL, doSomeThing, (void*)(intptr_t)atoi(argv[2]));
        if (err)
        {
            fprintf(stderr, "Can't create thread :[%s]", strerror(err));
            exit(EXIT_FAILURE);
        }
    }

    for (int i = 0; i < NUMT; i++)
        pthread_join(tid[i], NULL);

    pthread_mutex_destroy(&lock);

    printf("%d", counter);

    return 0;
}