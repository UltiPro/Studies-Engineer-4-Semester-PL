#include <stdio.h>
#include <stdlib.h>
#include <unistd.h>
#include <pthread.h>
#include <string.h>

void *printString(void *ptr);
void *printCounter();
int printCount = 0;
pthread_mutex_t count_mutex = PTHREAD_MUTEX_INITIALIZER;
pthread_cond_t count_var = PTHREAD_COND_INITIALIZER;
pthread_mutex_t mutex1 = PTHREAD_MUTEX_INITIALIZER;

int main()
{
    pthread_t thread1, thread2, thread3;
    int iret1, iret2, iret3;
    iret1 = pthread_create(&thread1, NULL, printString, "HELLO WORLD ");
    // error handling....
    iret2 = pthread_create(&thread2, NULL, printString, "Ala ma kota ");
    iret3 = pthread_create(&thread3, NULL, printCounter, NULL);
    pthread_join(thread1, NULL);
    pthread_join(thread2, NULL);
    pthread_join(thread3, NULL);
    exit(0);
}

void *printCounter()
{
    while (1)
    {
        pthread_mutex_lock(&count_mutex);
        pthread_cond_wait(&count_var, &count_mutex);
        printf("Wydrukowano znakow: %d\n", printCount);
        pthread_mutex_unlock(&count_mutex);
    }
}

void screenPrinter(char c)
{
    pthread_mutex_lock(&count_mutex);
    printf("%c\n", c);
    printCount++;
    if (printCount % 10 == 0)
        pthread_cond_signal(&count_var);
    pthread_mutex_unlock(&count_mutex);
}

void *printString(void *ptr)
{
    char *message;
    message = (char *)ptr;
    int len = strlen(message);
    int i = 0;
    while (1)
    {
        pthread_mutex_lock(&mutex1);
        for (i = 0; i < len; i++)
        {
            screenPrinter(message[i]);
            usleep(200 * 1000);
        }
        pthread_mutex_unlock(&mutex1);
        i = 0;
        sleep(1);
    }
}