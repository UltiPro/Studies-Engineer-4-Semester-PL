#include <pthread.h>
#include <stdio.h>
#include <unistd.h>
#include <stdlib.h>

#define NUMOFTHREADS 10

void *f(void *arg)
{
    int num = rand() % 10001;

    for (int i = 0; i < num; ++i)
    {
        printf("Thread #%ld, i = %d\n", (long)arg, i);
    }

    pthread_exit((void *)(intptr_t)num);
}

int main()
{
    pthread_t threads[NUMOFTHREADS];
    pthread_attr_t attr;

    pthread_attr_init(&attr);
    pthread_attr_setdetachstate(&attr, PTHREAD_CREATE_JOINABLE);

    int rc = 0;
    for (long i = 0; i < NUMOFTHREADS; ++i)
    {
        if ((rc = pthread_create(&threads[i], NULL, f, NULL)))
        {
            printf("Thread, bład utworzenia: %d\n", rc);
            exit(EXIT_FAILURE);
        }
    }

    pthread_attr_destroy(&attr);

    long sum = 0;
    for (int i = 0; i < NUMOFTHREADS; i++)
    {
        void *status;
        if (pthread_join(threads[i], &status))
        {
            printf("Thread, bład join'a");
            exit(EXIT_FAILURE);
        }
        sum += (long)status;
    }

    printf("Iterations: %ld\n", sum);
    pthread_exit(NULL);

    return 0;
}