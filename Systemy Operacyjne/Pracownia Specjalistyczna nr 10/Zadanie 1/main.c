#include <stdio.h>
#include <stdlib.h>
#include <stdint.h>
#include <pthread.h>

void *Fork(void *arg);

pthread_mutex_t mutex[5] = PTHREAD_MUTEX_INITIALIZER;

int main(int argc, char argv[])
{
    for (int i = 0; i < 5; i++)
    {
        if (pthread_mutex_init(&mutex[i], NULL) != 0)
        {
            printf("Error of mutex init: %d", i);
            return -1;
        }
    }

    pthread_t threads[5], threads2[5];
    int iret[5];

    for (int i = 0; i < 5; i++)
    {
        iret[i] = pthread_create(&threads[i], NULL, Fork, (void *)(intptr_t)i);

        if (iret[i])
        {
            fprintf(stderr, "Error - pthread_create() return code: %d\n", iret[i]);
            exit(EXIT_FAILURE);
        }

        iret[i] = pthread_create(&threads2[i], NULL, Fork, (void *)(intptr_t)((i + 1) % 5));
        if (iret[i])
        {
            fprintf(stderr, "Error - pthread_create() return code: %d\n", iret[i]);
            exit(EXIT_FAILURE);
        }
    }

    return 0;
}

void *Fork(void *arg)
{
    pthread_mutex_lock(&mutex[(int)(intptr_t)arg]);
    printf("Taken Fork number: %d\n", (int)(intptr_t)arg);
}
