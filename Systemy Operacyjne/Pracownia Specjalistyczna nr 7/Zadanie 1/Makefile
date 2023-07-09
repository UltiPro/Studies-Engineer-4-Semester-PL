OBJ = main.o
all: Zadanie
Zadanie: $(OBJ)
	gcc $(OBJ) -o Zadanie -pthread -lm -O3
.PHONY: clean
clean:
	rm -f *.o Zadanie