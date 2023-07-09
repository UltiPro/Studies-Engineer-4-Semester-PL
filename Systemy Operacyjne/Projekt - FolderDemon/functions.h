#include <sys/stat.h>
#include <sys/mman.h>
#include <stdio.h>
#include <stdlib.h>
#include <dirent.h>
#include <unistd.h>
#include <stdbool.h>
#include <signal.h>
#include <string.h>
#include <utime.h>
#include <fcntl.h>
#include <syslog.h>
#include <ctype.h>

void ChangeSettings(char *in, char *out);
void Delete(char *path_name, char *folder_path1, char *folder_path2, bool recursion);
void Copy(char *in, char *out);
void CopyMapping(char *in, char *out);
void FolderScrolling(char *path_name, char *folder_path1, char *folder_path2, bool recursion, int MaxSizeOfFile);
void Logging(int sig);
bool isNumber(char number[]);
bool IsDirectory(const char *path);
bool Checking(char *path_name, char *folder_path1, char *folder_path2);
char *ChangeFolder(char *path1, char *folder_path1, char *folder_path2);
char *AddToPath(char *path, char *add);
off_t CheckSize(char *in);
time_t CheckTime(char *in);
mode_t CheckCHMOD(char *in);