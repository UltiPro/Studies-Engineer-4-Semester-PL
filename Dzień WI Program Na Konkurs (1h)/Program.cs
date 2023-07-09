StreamReader ReadFile;

int numberOfRowsAndColumns = 1000;

int[,] field = new int[numberOfRowsAndColumns, numberOfRowsAndColumns];
int[,] positionsOfBombs = new int[numberOfRowsAndColumns * numberOfRowsAndColumns, 2]; //x and y
int numberOfBombs = 0;

if (File.Exists("minesweeper.txt"))
{
    ReadFile = new StreamReader("minesweeper.txt");

    for (int i = 0; i < numberOfRowsAndColumns; i++)
    {
        string? bufor = ReadFile.ReadLine();

        for (int j = 0; j < numberOfRowsAndColumns; j++)
        {
            if (bufor != null && bufor[j] == '*')
            {
                field[i, j] = -1;
                positionsOfBombs[numberOfBombs, 0] = i;
                positionsOfBombs[numberOfBombs, 1] = j;
                numberOfBombs++;
            }
        }

    }

    ReadFile.Close();

    for (int i = 0; i < numberOfBombs; i++)
    {
        SetBombs(positionsOfBombs[i, 0], positionsOfBombs[i, 1]);
    }

    var WriteFile = new System.IO.StreamWriter("solution.txt");

    for (int i = 0; i < numberOfRowsAndColumns; i++)
    {
        for (int j = 0; j < numberOfRowsAndColumns; j++)
        {
            if (field[i, j] == -1) WriteFile.Write('*');
            else WriteFile.Write(field[i, j]);
        }
        if (i != numberOfRowsAndColumns - 1) WriteFile.Write("\n");
    }

    WriteFile.Close();
}

void SetBombs(int x, int y)
{
    if (!IsThereBombOrNoSpace(x - 1, y - 1)) field[x - 1, y - 1] += 1;
    if (!IsThereBombOrNoSpace(x - 1, y)) field[x - 1, y] += 1;
    if (!IsThereBombOrNoSpace(x - 1, y + 1)) field[x - 1, y + 1] += 1;
    if (!IsThereBombOrNoSpace(x, y - 1)) field[x, y - 1] += 1;
    if (!IsThereBombOrNoSpace(x, y + 1)) field[x, y + 1] += 1;
    if (!IsThereBombOrNoSpace(x + 1, y - 1)) field[x + 1, y - 1] += 1;
    if (!IsThereBombOrNoSpace(x + 1, y)) field[x + 1, y] += 1;
    if (!IsThereBombOrNoSpace(x + 1, y + 1)) field[x + 1, y + 1] += 1;
}

bool IsThereBombOrNoSpace(int x, int y)
{
    if (x == -1 || x == numberOfRowsAndColumns) return true;
    if (y == -1 || y == numberOfRowsAndColumns) return true;
    if (field[x, y] == -1) return true;
    return false;
}