#include <stdio.h>
#include <sys/wait.h>
#include <unistd.h>
#include <stdlib.h>
#include <fcntl.h>
#include <assert.h>

int main(int argc, char *argv[])
{
    if (argc < 2)
    {
        fprintf(stderr, "Użycie programu: nazwa_programu <nazwa_pliku_do_zapisu>");
        exit(EXIT_FAILURE);
    }

    int fds[2];

    if (pipe(fds) == -1)
    {
        perror("PIPE ERROR");
        exit(EXIT_FAILURE);
    }

    pid_t pid = fork();

    if (pid == (pid_t)0)
    {
        if (close(fds[1]))
        {
            perror("Bład zamknięcia potoku 1 procesu dziecka");
            exit(EXIT_FAILURE);
        }

        if (dup2(fds[0], STDIN_FILENO) < 0)
        {
            perror("Bład dup2  w procesie dziecka");
            exit(EXIT_FAILURE);
        }

        int fd;
        if(!(fd = open(argv[1], O_WRONLY | O_CREAT, S_IRWXU)))
        {
            perror("Bład otwarcia fd");
            exit(EXIT_FAILURE);
        }

        assert(fd);

        if (dup2(fd, STDOUT_FILENO) < 0)
        {
            perror("Bład dup2  w procesie dziecka");
            exit(EXIT_FAILURE);
        }

        if(execlp("sort", "sort", NULL) < 0)
        {
            perror("Bład execlp");
            exit(EXIT_FAILURE);
        }
    }
    else if (pid < (pid_t)0)
    {
        perror("Błąd forka");
        return EXIT_FAILURE;
    }
    else
    {
        if (close(fds[0]) == -1)
        {
            perror("Błąd zamknięcia 0 potoku");
            exit(EXIT_FAILURE);
        }

        FILE *stream;

        if (!(stream = fdopen(fds[1], "w")))
        {
            perror("Błąd pipa do zapisu");
            exit(EXIT_FAILURE);
        }

        fprintf(stream, "This is a test.\n");
        fprintf(stream, "Hello, world.\n");
        fprintf(stream, "My dog has fleas.\n");
        fprintf(stream, "This program is great.\n");
        fprintf(stream, "One fish, two fish.\n");

        fflush(stream);

        if (close(fds[1]) == -1)
        {
            perror("Błąd zamknięcia 1 potoku");
            exit(EXIT_FAILURE);
        }

        if ((waitpid(pid, NULL, 0)) == -1)
        {
            perror("Błąd waitpida!");
            exit(EXIT_FAILURE);
        }
    }
    return 0;
}