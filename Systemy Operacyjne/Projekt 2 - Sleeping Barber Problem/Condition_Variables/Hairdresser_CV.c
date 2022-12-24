#include <pthread.h>
#include <stdio.h>
#include <stdlib.h>
#include <unistd.h>
#include <getopt.h>
#include <stdbool.h>
#include <ctype.h>

struct List // Lista do kolejki FIFO
{
    int client_number;
    struct List *next;
};

pthread_cond_t Free_Seats;         // Dostępne miejsca w poczekalni
pthread_cond_t Busy_Hairdresser;   // Fryzjer nie jest dostępny
pthread_cond_t WakeUp_Hairdresser; // Budzenie fryzjera
pthread_cond_t Free_Hairdresser;   // Fryzjer skończył strzyc

pthread_mutex_t Seat;         // Fotel u fryzjera
pthread_mutex_t Waiting_Room; // Mutex do ochrony miejsc w poczekalni
pthread_mutex_t Client;       // Klient po strzyżeniu
pthread_mutex_t Hairdresser;  // Mutex do ochrony stanu pracy/spania fryzjera

bool Done = false;      // Flaga końca pracy
bool Busy_Seat = false; // Flaga zajętego fotela u fryzjera
bool Debug;             // Flaga do wypisywania list

int Count_Of_Seats = 5;            // Dostepana liczba miejsc w poczekalni
int Count_Of_Free_Seats;           // Liczba wolnych miejsc w poczekalni
int Count_Of_Resignation = 0;      // Liczba osob ktora zrezygnowała ze strzyżenia
int Hairdresser_Time_To_Clip = 10; // Tempo strzyżenia klienta od 1 sec do Time_To_Clip
int Client_Time_To_Come = 5;       // Czas klienta przyjścia do salonu od otwarcia -> od 1 sec do Client_Time_To_Come
int Current_Client = -1;           // Zmienna przetrzymująca aktualnego klienta na fotelu; -1 gdy nie ma klienta
int Time_Bufor = 0;                // Bufor czasu do odczekania między klientami

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

int First_FIFO() // Pierwszy w kolejce
{
    struct List *temp = Waitting;
    while (temp->next != NULL)
        temp = temp->next;
    return temp->client_number;
}

void Remove_First_FIFO() // Usuwa pierwszego w kolejce
{
    struct List *temp = Waitting;
    while (temp->next->next != NULL)
        temp = temp->next;
    free(temp->next);
    temp->next = NULL;
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
            Add_Waitting(nr);                // Dodanie klienta do poczekalni
        pthread_mutex_unlock(&Waiting_Room); // Odblokowanie Poczekalni

        // Czekanie aż fotel się zwolni
        pthread_mutex_lock(&Seat);
        if (Busy_Seat)
            pthread_cond_wait(&Busy_Hairdresser, &Seat);
        Busy_Seat = true;
        pthread_mutex_unlock(&Seat);

        // Klient idzie na fotel fryzjerski
        pthread_mutex_lock(&Waiting_Room);
        Count_Of_Free_Seats++;
        Current_Client = nr;
        printf("Res:%d WRomm: %d/%d [in: %d] - START\n", Count_Of_Resignation, Count_Of_Seats - Count_Of_Free_Seats, Count_Of_Seats, Current_Client);
        if (Debug == true)
            Remove_Client(nr); // Usunięcie klienta z kolejki
        pthread_mutex_unlock(&Waiting_Room);

        // Budzenie Fryzjera
        pthread_cond_signal(&WakeUp_Hairdresser);

        // Czekanie aż ścinanie się skończy
        pthread_mutex_lock(&Client);
        pthread_cond_wait(&Free_Hairdresser, &Client);
        pthread_mutex_unlock(&Client);

        // Odlbkowanie fotela i przekazanie inforacji o tym że fryzjer jest wolny
        pthread_mutex_lock(&Seat);
        Busy_Seat = false;
        pthread_mutex_unlock(&Seat);
        pthread_cond_signal(&Busy_Hairdresser);
    }
    else
    {
        Count_Of_Resignation++; // Zwiększenie liczby osób które zrezygnowały
        printf("Res:%d WRomm: %d/%d [in: %d] - Client didn't get a chair! All busy!\n", Count_Of_Resignation, Count_Of_Seats - Count_Of_Free_Seats, Count_Of_Seats, Current_Client);
        if (Debug == true)
            Add_Rejected(nr);                // Dodanie do listy osób które się nie dostały
        pthread_mutex_unlock(&Waiting_Room); // Odblokowanie Poczekalni
    }
}

void *Hairdresser_Fun()
{
    do
    {
        pthread_mutex_lock(&Hairdresser);                     // Zablokowanie Fryzjera
        pthread_cond_wait(&WakeUp_Hairdresser, &Hairdresser); // Czekamy na wybudzenie Fryzjera
        pthread_mutex_unlock(&Hairdresser);                   // Odblokowywujemy Fryzjeraclear
        Wait(0);                                              // Klient jest obsługiwany
        if (Busy_Seat)
        {
            printf("Res:%d WRomm: %d/%d [in: %d] - END\n", Count_Of_Resignation, Count_Of_Seats - Count_Of_Free_Seats, Count_Of_Seats, Current_Client);
            Wait(0); // Sprzątanie po kliencie
        }
        pthread_cond_signal(&Free_Hairdresser); // Koniec strzyżenia
    } while (!Done);                            // Póki są klienci
    printf("Koniec pracy...\n");
}

int main(int argc, char *argv[])
{
    Count_Of_Free_Seats = Count_Of_Seats;
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

    // Inicjalizacja zmiennych warunkowych
    pthread_cond_init(&Free_Seats, NULL);
    pthread_cond_init(&Busy_Hairdresser, NULL);
    pthread_cond_init(&WakeUp_Hairdresser, NULL);
    pthread_cond_init(&Free_Hairdresser, NULL);

    // Inicjalizacja mutexów
    pthread_mutex_init(&Seat, NULL);
    pthread_mutex_init(&Waiting_Room, NULL);
    pthread_mutex_init(&Client, NULL);
    pthread_mutex_init(&Hairdresser, NULL);

    // Tworzenie wątku fryzjera
    pthread_create(&Hairdresser_Theard, 0, Hairdresser_Fun, 0);

    // Tworzenie wątków klientów
    for (int i = 0; i < CoutOfClients; ++i)
        pthread_create(&Clients_Theards[i], 0, Client_Fun, (void *)(intptr_t)(i));

    // Obsługa wszystkich klientów
    for (int i = 0; i < CoutOfClients; i++)
        pthread_join(Clients_Theards[i], 0);

    // Flaga końca pracy
    Done = true;

    // Budzenie fryzjera, koniec pracy
    pthread_cond_signal(&WakeUp_Hairdresser);
    pthread_join(Hairdresser_Theard, 0);

    // Czyszczenie
    pthread_cond_destroy(&Free_Seats);
    pthread_cond_destroy(&Busy_Hairdresser);
    pthread_cond_destroy(&WakeUp_Hairdresser);
    pthread_cond_destroy(&Free_Hairdresser);

    pthread_mutex_destroy(&Seat);
    pthread_mutex_destroy(&Waiting_Room);
    pthread_mutex_destroy(&Client);
    pthread_mutex_destroy(&Hairdresser);

    free(Waitting);
    free(Rejected);

    return 0;
}