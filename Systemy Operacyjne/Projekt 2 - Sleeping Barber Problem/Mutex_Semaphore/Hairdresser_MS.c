#include <stdio.h>
#include <stdlib.h>
#include <semaphore.h>
#include <pthread.h>
#include <unistd.h>
#include <stdbool.h>
#include <getopt.h>
#include <unistd.h>
#include <ctype.h>

struct List // Lista do kolejki FIFO
{
    int client_number;
    struct List *next;
};

sem_t Clients;     // Liczba klientów czekających na strzyżenie => 0 brak osób w poczekalni
sem_t Hairdresser; // Flaga zajętości fryzjera

pthread_mutex_t Seat;         // Fotel u fryzjera
pthread_mutex_t Waiting_Room; // Mutex do ochrony miejsc w poczekalni

int Count_Of_Seats = 5;            // Dostepana liczba miejsc w poczekalni
int Count_Of_Free_Seats;           // Liczba wolnych miejsc w poczekalni
int Count_Of_Resignation = 0;      // Liczba osob ktora zrezygnowała ze strzyżenia
int Hairdresser_Time_To_Clip = 10; // Tempo strzyżenia klienta od 1 sec do Time_To_Clip
int Client_Time_To_Come = 5;       // Czas klienta przyjścia do salonu od otwarcia -> od 1 sec do Client_Time_To_Come
int Current_Client = -1;           // Zmienna przetrzymująca aktualnego klienta na fotelu; -1 gdy nie ma klienta
int Time_Bufor = 0;                // Bufor czasu do odczekania między klientami

bool Done = false;  // Flaga końca pracy
bool Debug = false; // Flaga do wypisywania list

struct List *Rejected = NULL; // Lista klientów którzy nie dostali się do poczekalni
struct List *Waitting = NULL; // Lista klientów czekajacych w poczekalni

bool isNumber(char number[]) // Sprawdzenie czy parametr jest konwertowalny na liczbę
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

void Wait(bool client) // Funkcja do losowego czekania
{
    srand(time(NULL) ^ (getpid() << 16));
    if (client)
    {
        int x = ((rand() % Client_Time_To_Come) + 1);
        Time_Bufor += x + 2 * (Client_Time_To_Come % x);
        sleep(Time_Bufor);
    }
    else
    {
        int x = ((rand() % Hairdresser_Time_To_Clip) + 1);
        sleep(x);
    }
}

void Show(struct List *data)
{
    struct List *temp = data;
    while (temp != NULL)
    {
        printf("%d ", temp->client_number);
        temp = temp->next;
    }
    printf("\n");
}

struct List *Add(int client, struct List *target)
{
    struct List *temp = (struct List *)malloc(sizeof(struct List));
    temp->client_number = client;
    temp->next = target;
    target = temp;
    return target;
}

void Add_Rejected(int client) // Dodaj odrzuconego klienta
{
    Rejected = Add(client, Rejected);
    printf("They didn't come in: ");
    Show(Rejected);
}

void Add_Waitting(int client) // Dodaj czekającego klienta
{
    Waitting = Add(client, Waitting);
    printf("They are waiting: ");
    Show(Waitting);
}

void Remove_Client(int client) // Usuwa klienta z kolejki
{
    struct List *temp = Waitting;
    struct List *pop = Waitting;
    while (temp != NULL)
    {
        if (temp->client_number == client)
        {
            if (temp->client_number == Waitting->client_number)
                Waitting = Waitting->next;
            else
                pop->next = temp->next;
            free(temp);
            break;
        }
        pop = temp;
        temp = temp->next;
    }
    printf("They are waiting: ");
    Show(Waitting);
}

void *Client_Fun(void *number_of_client)
{
    Wait(1); // Czekanie na przyjście klienta
    int nr = (int)(intptr_t)number_of_client;
    pthread_mutex_lock(&Waiting_Room); // Blokowanie poczekalni
    if (Count_Of_Free_Seats > 0)       // Klient wchodzi jeżeli są wolne miejsca
    {
        Count_Of_Free_Seats--; // Zmniejszenie wolnych miejsc w poczekalni
        printf("Res:%d WRomm: %d/%d [in: %d] - Client get a chair!\n", Count_Of_Resignation, Count_Of_Seats - Count_Of_Free_Seats, Count_Of_Seats, Current_Client);
        if (Debug == true)
            Add_Waitting(nr);
        sem_post(&Clients);                  // Sygnał dla fryzjera że klient jest w poczekalni
        pthread_mutex_unlock(&Waiting_Room); // Odblokowanie Poczekalni
        sem_wait(&Hairdresser);              // Czekania na wykonanie usługi przez fryzjera
        pthread_mutex_lock(&Seat);           // Blokada fotela
        Current_Client = nr;                 // Pobranie numeru klienta z fotela
        printf("Res:%d WRomm: %d/%d [in: %d] - START\n", Count_Of_Resignation, Count_Of_Seats - Count_Of_Free_Seats, Count_Of_Seats, Current_Client);
        if (Debug == true)
            Remove_Client(nr);
    }
    else
    {
        pthread_mutex_unlock(&Waiting_Room); // Odblokowanie Poczekalni
        Count_Of_Resignation++;
        printf("Res:%d WRomm: %d/%d [in: %d] - Client didn't get a chair! All busy!\n", Count_Of_Resignation, Count_Of_Seats - Count_Of_Free_Seats, Count_Of_Seats, Current_Client);
        if (Debug == true)
            Add_Rejected(nr);
    }
}

void *Hairdresser_Fun()
{
    while (!Done)
    {
        if (!Done)
        {
            sem_wait(&Clients); // Fryzjer czeka na ukazanie się klienta
            pthread_mutex_lock(&Waiting_Room);
            Count_Of_Free_Seats++;
            pthread_mutex_unlock(&Waiting_Room);
            sem_post(&Hairdresser); // Fryzjer jest gotowy do ścinania
            Wait(0);                // Wykonywanie usługi
            printf("Res:%d WRomm: %d/%d [in: %d] - END\n", Count_Of_Resignation, Count_Of_Seats - Count_Of_Free_Seats, Count_Of_Seats, Current_Client);
            Wait(0);                     // Sprzątanie po kliencie
            pthread_mutex_unlock(&Seat); // Zwolnienie miejsca na fotelu
        }
    }
    printf("Koniec pracy...\n");
}

int main(int argc, char *argv[])
{
    Count_Of_Free_Seats = Count_Of_Seats;
    srand(time(NULL));
    static struct option params[] =
        {
            {"c", required_argument, NULL, 'c'},
            {"s", required_argument, NULL, 's'},
            {"t", required_argument, NULL, 't'},
            {"f", required_argument, NULL, 'f'},
            {"d", no_argument, NULL, 'd'}};

    int CoutOfClients = 15; // liczba klientow ktorzy sie pojawia

    int choose = 0;
    while ((choose = getopt_long(argc, argv, "c:s:t:f:d", params, NULL)) != -1)
    {
        switch (choose)
        {
        case 'c': // Max liczba klientów
            if (!(isNumber(optarg)))
            {
                printf("Error Syntax: -c argument has to be a number!");
                exit(EXIT_FAILURE);
            }
            CoutOfClients = atoi(optarg);
            break;
        case 's': // Liczba krzeseł w poczekalni
            if (!(isNumber(optarg)))
            {
                printf("Error Syntax: -s argument has to be a number!");
                exit(EXIT_FAILURE);
            }
            Count_Of_Free_Seats = atoi(optarg);
            Count_Of_Seats = atoi(optarg);
            break;
        case 't': // Czas pojawiania się klientów w salonie
            if (!(isNumber(optarg)))
            {
                printf("Error Syntax: -t argument has to be a number!");
                exit(EXIT_FAILURE);
            }
            Client_Time_To_Come = atoi(optarg);
            break;
        case 'f': // Szybkosc scinania fryzjera
            if (!(isNumber(optarg)))
            {
                printf("Error Syntax: -f argument has to be a number!");
                exit(EXIT_FAILURE);
            }
            Hairdresser_Time_To_Clip = atoi(optarg);
            break;
        case 'd':
            Debug = true;
            break;
        }
    }

    pthread_t *Clients_Theards = malloc(sizeof(pthread_t) * CoutOfClients);
    pthread_t Hairdresser_Theard;

    // Inicjalizacja Semaforów
    sem_init(&Clients, 0, 0);
    sem_init(&Hairdresser, 0, 0);

    // Inicjalizacja mutexów
    pthread_mutex_init(&Seat, NULL);
    pthread_mutex_init(&Waiting_Room, NULL);

    // Tworzenie wątku fryzjera
    pthread_create(&Hairdresser_Theard, NULL, Hairdresser_Fun, NULL);

    // Tworzenie wątków klientów
    for (int i = 0; i < CoutOfClients; ++i)
        pthread_create(&Clients_Theards[i], 0, Client_Fun, (void *)(intptr_t)(i));

    // Obsługa wszystkich klientów
    for (int i = 0; i < CoutOfClients; i++)
        pthread_join(Clients_Theards[i], 0);

    // Flaga końca pracy
    Done = true;

    // Budzenie fryzjera, koniec pracy
    pthread_join(Hairdresser_Theard, 0);

    // Czyszczenie
    pthread_mutex_destroy(&Seat);
    pthread_mutex_destroy(&Waiting_Room);

    sem_destroy(&Clients);
    sem_destroy(&Hairdresser);

    free(Waitting);
    free(Rejected);

    return 0;
}