OBJ = main.o functions.o
all: FolderDemon
FolderDemon: $(OBJ)
	gcc $(OBJ) -o FolderDemon
$(OBJ): functions.h
.PHONY: clean
clean:
	rm -f *.o FolderDemon