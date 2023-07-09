#include <sys/types.h>
#include "functions.h"

#define DEFAULT_TIME_TO_SLEEP 300
#define DEFAULT_SIZE_OF_FILE 1024

int main(int argc, char **argv)
{
    openlog("FolderDemon", LOG_PID | LOG_CONS, LOG_USER);

    if (argc < 3)
    {
        printf("Error Syntax: Too few input arguments! Basic usage: ./FolderDemon <<path_copy_from>> <<path_copy_to>>");
        syslog(LOG_ERR, "Error Syntax: Too few input arguments!");
        exit(EXIT_FAILURE);
    }

    char *path_copy_from = argv[1];
    char *path_copy_to = argv[2];

    if (!(strcmp(path_copy_from, path_copy_to)))
    {
        printf("Error Syntax: The both paths are the same! Basic usage: ./FolderDemon <<path_copy_from>> <<path_copy_to>>");
        syslog(LOG_ERR, "Error Syntax: Given path are these same!");
        exit(EXIT_FAILURE);
    }

    if (!(IsDirectory(path_copy_from)))
    {
        printf("Error Syntax: The <<path_to_check>> is incorrect! Basic usage: ./FolderDemon <<path_copy_from>> <<path_copy_to>>");
        syslog(LOG_ERR, "Error Syntax: Given argument is not a directory!");
        exit(EXIT_FAILURE);
    }

    if (!(IsDirectory(path_copy_to)))
    {
        printf("Error Syntax: The <<path_to_copy>> is incorrect! Basic usage: ./FolderDemon <<path_copy_from>> <<path_copy_to>>");
        syslog(LOG_ERR, "Error Syntax: Given argument is not a directory!");
        exit(EXIT_FAILURE);
    }

    int choose = 0, size = DEFAULT_SIZE_OF_FILE, sleep_time = DEFAULT_TIME_TO_SLEEP;
    int recursion = 0;

    while ((choose = getopt(argc, argv, "s:rm:")) != -1)
    {
        switch (choose)
        {
        case 's':
            if (!(isNumber(optarg)))
            {
                printf("Error Syntax: -s argument has to be a number!");
                syslog(LOG_ERR, "Error Syntax: -s argument has to be a number!");
                exit(EXIT_FAILURE);
            }
            sleep_time = atoi(optarg);
            break;
        case 'r':
            recursion = 1;
            break;
        case 'm':
            if (!(isNumber(optarg)))
            {
                printf("Error Syntax: -m argument has to be a number!");
                syslog(LOG_ERR, "Error Syntax: -m argument has to be a number!");
                exit(EXIT_FAILURE);
            }
            size = atoi(optarg);
            break;
        }
    }

    pid_t pid = fork();

    if (pid < 0)
    {
        syslog(LOG_ERR, "Incorrect ID of child process!");
        exit(EXIT_FAILURE);
    }
    else if (pid > 0)
        exit(EXIT_SUCCESS);

    umask(0);

    pid_t sid = setsid();

    if (sid < 0)
    {
        syslog(LOG_ERR, "Error of Session ID!");
        exit(EXIT_FAILURE);
    }

    if ((chdir("/")) < 0)
    {
        syslog(LOG_ERR, "Error with changing directory!");
        exit(EXIT_FAILURE);
    }

    close(STDIN_FILENO);
    close(STDOUT_FILENO);
    close(STDERR_FILENO);

    syslog(LOG_INFO, "FolderDemon - start");

    if (signal(SIGUSR1, Logging) == SIG_ERR)
    {
        syslog(LOG_ERR, "Error of Signal SIGUSR1!");
        exit(EXIT_FAILURE);
    }

    while (1)
    {
        Delete(path_copy_to, path_copy_from, path_copy_to, recursion);
        FolderScrolling(path_copy_from, path_copy_from, path_copy_to, recursion, size);
        syslog(LOG_INFO, "FolderDemon has felt asleep!");
        if ((sleep(sleep_time)) == 0)
            syslog(LOG_INFO, "FolderDemon has wake up!");
    }

    closelog();

    return 0;
}