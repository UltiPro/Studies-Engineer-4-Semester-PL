#include <string.h>
#include <signal.h>
#include <stdio.h>
#include <stdlib.h>
#include <unistd.h>
#include <syslog.h>

volatile sig_atomic_t keep_going = 1;

void catch_alarm(int signum)
{
    keep_going = 0;
    signal(signum, catch_alarm);
    printf("%s\n", strsignal(signum)); // Quit communicat for CTRL + \ "
    syslog(LOG_NOTICE, "Program Zadanie został zakończony"); 
    // Zapis do logu po użyciu CTRL + L
}

void signal_show()
{
    printf("signal!\n");
}

int main(void)
{
    syslog(LOG_NOTICE, "Program Zadanie wystartowało, uruchomił to użytkownik o id %d", getuid());
    // Zapis do logu systemowego informacji o uruchomieniu programu

    __sighandler_t error = signal(SIGQUIT, catch_alarm);

    if (error == SIG_ERR)
    {
        perror("Blad parametru signal");
        return 0;
    }

    while (keep_going)
    {
        signal_show();
        sleep(1);
    }

    return 0;
}