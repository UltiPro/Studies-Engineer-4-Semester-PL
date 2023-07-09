#include <stdio.h>
#include <stdlib.h>
#include "read.h"

int main(int argc, char* argv[])
{
    if(argc<2) 
    {
        printf("Nie podano żadnego pliku!\n");
        return 1;
    }

    for(int i=1;i<argc;i++)
    {
        czytaj(argv[i]);
        if(i!=argc-1) printf("\n");
    } 
    
    return 0;
}