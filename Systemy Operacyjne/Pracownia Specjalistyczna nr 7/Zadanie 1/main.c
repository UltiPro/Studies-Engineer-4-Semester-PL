#include <stdio.h>
#include <stdlib.h>
#include <pthread.h>

pthread_mutex_t mutex1 = PTHREAD_MUTEX_INITIALIZER;
volatile int counter = 0;

void *f1()
{
    while (1)
    {
        pthread_mutex_lock(&mutex1);
        counter++;
        pthread_mutex_unlock(&mutex1);
    }
}

void *f2()
{
    while (1)
    {
        pthread_mutex_lock(&mutex1);
        printf("Counter value: %d\n", counter);
        pthread_mutex_unlock(&mutex1);
    }
}

int main()
{
    int rc1, rc2;
    pthread_t thread1, thread2;

    if ((rc1 = pthread_create(&thread1, NULL, &f1, NULL)))
    {
        printf("Thread, bład utworzenia: %d\n", rc1);
    }

    if ((rc2 = pthread_create(&thread2, NULL, &f2, NULL)))
    {
        printf("Thread, bład utworzenia: %d\n", rc2);
    }

    pthread_join(thread1, NULL);
    pthread_join(thread2, NULL);

    return 0;
}
