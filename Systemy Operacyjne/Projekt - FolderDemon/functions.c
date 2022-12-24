#include "functions.h"

void ChangeSettings(char *in, char *out)
{
    struct utimbuf time;
    time.actime = 0;
    time.modtime = CheckTime(in);
    if (utime(out, &time) != 0)
    {
        syslog(LOG_ERR, "Error of modification date.");
        exit(EXIT_FAILURE);
    }
    mode_t old = CheckCHMOD(in);
    if (chmod(out, old) != 0)
    {
        syslog(LOG_ERR, "Error of setting date of modification.");
        exit(EXIT_FAILURE);
    }
}

void Delete(char *path_name, char *folder_path1, char *folder_path2, bool recursion)
{
    struct dirent *file;
    DIR *path, *help_path;
    path = opendir(path_name);
    while ((file = readdir(path)))
    {
        char *new_path = AddToPath(path_name, file->d_name);
        if ((file->d_type) == DT_DIR)
        {
            if (recursion && (!(strcmp(file->d_name, ".") == 0 || strcmp(file->d_name, "..") == 0)))
            {
                Delete(new_path, folder_path1, folder_path2, recursion);
                if (!(help_path = opendir(ChangeFolder(new_path, folder_path1, folder_path2))))
                {
                    syslog(LOG_INFO, "Removed directory: %s.", new_path);
                    remove(new_path);
                }
                else
                    closedir(help_path);
            }
        }
        else
        {
            if (access(ChangeFolder(new_path, folder_path1, folder_path2), F_OK) == -1)
            {
                syslog(LOG_INFO, "Removed file %s.", new_path);
                remove(new_path);
            }
        }
    }
    closedir(path);
}

void Copy(char *in, char *out)
{
    char bufor[16];
    int infile, outfile;
    int readin, readout;
    infile = open(in, O_RDONLY);
    outfile = open(out, O_CREAT | O_WRONLY | O_TRUNC, 0644);
    if (infile == -1 || outfile == -1)
    {
        syslog(LOG_ERR, "Error opening file.");
        exit(EXIT_FAILURE);
    }
    while ((readin = read(infile, bufor, sizeof(bufor))) > 0)
    {
        readout = write(outfile, bufor, (ssize_t)readin);
        if (readout != readin)
        {
            perror("Error.");
            exit(EXIT_FAILURE);
        }
    }
    close(infile);
    close(outfile);
    ChangeSettings(in, out);
    syslog(LOG_INFO, "File copied: %s.", in);
}

void CopyMapping(char *in, char *out)
{
    int size = CheckSize(in);
    int infile = open(in, O_RDONLY);
    int outfile = open(out, O_CREAT | O_WRONLY | O_TRUNC, 0644);
    if (infile == -1 || outfile == -1)
    {
        syslog(LOG_ERR, "Error opening file.");
        exit(EXIT_FAILURE);
    }
    char *map = (char *)mmap(0, size, PROT_READ, MAP_SHARED | MAP_FILE, infile, 0);
    write(outfile, map, size);
    close(infile);
    close(outfile);
    munmap(map, size);
    ChangeSettings(in, out);
    syslog(LOG_INFO, "Copied file: %s, using mapping to directory: %s.", in, out);
}

void FolderScrolling(char *path_name, char *folder_path1, char *folder_path2, bool recursion, int MaxSizeOfFile)
{
    struct dirent *file;
    DIR *path, *help_path;
    path = opendir(path_name);
    char *new_path;
    while ((file = readdir(path)))
    {
        if ((file->d_type) == DT_DIR)
        {
            if (recursion && (!(strcmp(file->d_name, ".") == 0 || strcmp(file->d_name, "..") == 0)))
            {
                char *path_to_folder = ChangeFolder(AddToPath(path_name, file->d_name), folder_path2, folder_path1);
                if (!(help_path = opendir(path_to_folder)))
                {
                    syslog(LOG_INFO, "Created folder: %s.", path_to_folder);
                    mkdir(path_to_folder, S_IRWXU | S_IRWXG | S_IROTH | S_IXOTH);
                }
                else
                    closedir(help_path);
                new_path = AddToPath(path_name, file->d_name);
                FolderScrolling(new_path, folder_path1, folder_path2, recursion, MaxSizeOfFile);
            }
        }
        else if ((file->d_type) == DT_REG)
        {
            new_path = AddToPath(path_name, file->d_name);
            int i;
            if ((i = Checking(new_path, folder_path1, folder_path2)) == 1)
            {
                if (CheckSize(new_path) > MaxSizeOfFile)
                    CopyMapping(new_path, ChangeFolder(new_path, folder_path2, folder_path1));
                else
                    Copy(new_path, ChangeFolder(new_path, folder_path2, folder_path1));
            }
        }
    }
    closedir(path);
}

void Logging(int sig)
{
    syslog(LOG_INFO, "FolderDemon wake up by signal SIGUSR1.");
}

bool isNumber(char number[])
{
    int i = 0;

    if (number[0] == '-')
        return false;
    for (; number[i] != 0; i++)
    {
        if (!isdigit(number[i]))
            return false;
    }
    return true;
}

bool IsDirectory(const char *path)
{
    struct stat stat_path;
    stat(path, &stat_path);
    return S_ISDIR(stat_path.st_mode);
}

bool Checking(char *path_name, char *folder_path1, char *folder_path2)
{
    bool decision = false;
    char *path_name_zm = path_name + strlen(folder_path1);
    char *lookfor = malloc(strlen(path_name_zm));
    char *new_path = ChangeFolder(path_name, folder_path2, folder_path1);
    int i = strlen(new_path);
    for (i; new_path[i] != '/'; i--)
        ;
    strcpy(lookfor, new_path + i + 1);
    new_path[i] = '\0';
    struct dirent *file;
    DIR *path;
    path = opendir(new_path);
    while ((file = readdir(path)))
    {
        if (strcmp(file->d_name, lookfor) == 0)
        {
            free(lookfor);
            if ((file->d_type) == DT_DIR)
                return 0;
            else
            {
                if ((int)CheckTime(path_name) == (int)CheckTime(AddToPath(new_path, file->d_name)))
                    return 0;
                else
                    return 1;
            }
        }
        else
            decision = true;
    }
    closedir(path);
    return decision;
}

char *ChangeFolder(char *path1, char *folder_path1, char *folder_path2)
{
    char *path = path1 + strlen(folder_path2);
    char *new_path = malloc(strlen(folder_path1) + strlen(path) + 1);
    strcpy(new_path, folder_path1);
    strcat(new_path, path);
    return new_path;
}

char *AddToPath(char *path, char *add)
{
    char *new_path = malloc(strlen(path) + 2 + strlen(add));
    strcpy(new_path, path);
    strcat(new_path, "/");
    strcat(new_path, add);
    new_path[strlen(path) + 1 + strlen(add)] = '\0';
    return new_path;
}

off_t CheckSize(char *in)
{
    struct stat size;
    if (stat(in, &size) == 0)
        return size.st_size;
    return -1;
}

time_t CheckTime(char *in)
{
    struct stat time;
    if (stat(in, &time) == -1)
    {
        syslog(LOG_ERR, "Error with checking edition date of file: %s.", in);
        exit(EXIT_FAILURE);
    }
    return time.st_mtime;
}

mode_t CheckCHMOD(char *in)
{
    struct stat mod;
    if (stat(in, &mod) == -1)
    {
        syslog(LOG_ERR, "Error with checking chmod of file: %s.", in);
        exit(EXIT_FAILURE);
    }
    return mod.st_mode;
}