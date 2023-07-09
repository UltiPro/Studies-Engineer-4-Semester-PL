#include <string.h>
#include <signal.h>
#include <stdio.h>
#include <stdlib.h>
#include <unistd.h>

void handler(int signum){
    printf("\nsignal!\n");
    printf("%s\n",strsignal(signum));
    exit(signum); // Brak exit'a powoduje iż CTRL + C nie zadziała 
                  // i trzeba zakończyć proces z poziomu terminala
}

int main(void)
{
    __sighandler_t error = signal(SIGINT, handler);

    if(error==SIG_ERR) // Wywołanie signal z parametrem SIGKILL zostanie tutaj wyłapane
    {
        perror("Blad parametru signal");
        return 0;
    }

    while(1)
    {
        printf("working...\n");
        sleep(1);
    }

    return 0;
}