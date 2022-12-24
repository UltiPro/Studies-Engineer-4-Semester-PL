#include <stdio.h>
#include <stdlib.h>
#include <fcntl.h>
#include <unistd.h> 
#include "read.h"

void czytaj(char *f)
{
    //Niskopoziomowy dostęp:

    unsigned char buffer[1]; 
    size_t bytes_read; 
    
    int file = open (f, O_RDONLY); 
    
    do 
    {
        bytes_read = read (file, buffer, sizeof (buffer)); 
        for (int i = 0; i < bytes_read; ++i) printf ("%c", buffer[i]); 
    } 
    while (bytes_read == sizeof (buffer)); 
    
    close(file);

    //Wysokopoziomowy dostęp

    /*
    FILE *file;
    char c;

    file = fopen(f, "r");
    if (file == NULL)
    {
        printf("Nie można otworzyć pliku \n");
        return;
    }

    c = fgetc(file);
    while (c != EOF)
    {
        printf ("%c", c);
        c = fgetc(file);
    }
    
    fclose(file);
    */
}