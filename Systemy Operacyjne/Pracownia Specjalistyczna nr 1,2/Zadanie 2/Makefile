OBJ = main.o read.o
all: czytaj
czytaj: $(OBJ)
	gcc $(OBJ) -o czytaj
$(OBJ): read.h
.PHONY: clean
clean:
	rm -f *.o czytaj