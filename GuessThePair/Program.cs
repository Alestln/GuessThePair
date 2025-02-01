namespace ConsoleApp1test
{
    internal class Program
    {
        static (int Rows, int Cols) GetFieldSize()
        {
            string errorMessage = "Ошибка! Введите число.";

            while (true)
            {
                Console.Write("Введите количество строк: ");
                if (!int.TryParse(Console.ReadLine(), out int rows))
                {
                    Console.WriteLine(errorMessage);
                    continue;
                }

                Console.Write("Введите количество столбцов: ");
                if (!int.TryParse(Console.ReadLine(), out int cols))
                {
                    Console.WriteLine(errorMessage);
                    continue;
                }

                if (rows * cols % 2 != 0)
                {
                    Console.WriteLine("Количество карт должно быть четным.");
                    continue;
                }

                return (rows, cols);
            }
        }

        static int[,] InitializeCards(int rows, int cols)
        {
            int[,] cards = new int[rows, cols];
            int[] pairs = GeneratePairs(rows * cols);

            int index = 0;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    cards[i, j] = pairs[index];
                    index++;
                }
            }

            return cards;
        }

        static int[] GeneratePairs(int count)
        {
            int[] pairs = new int[count];

            for (int i = 0; i < count / 2; i++)
            {
                pairs[i * 2] = i + 1;
                pairs[i * 2 + 1] = i + 1;
            }

            Random.Shared.Shuffle(pairs);

            return pairs;
        }

        static (int Row, int Col) GetPosition((int Rows, int Cols) size, bool[,] revealed)
        {
            while (true)
            {
                Console.Write("Введите номер строки и столбца через пробел: ");
                string input = Console.ReadLine().Trim();
                string[] numbers = input.Split(' ');

                if (numbers.Length != 2)
                {
                    Console.WriteLine("Нужно ввести два числа.\n");
                    continue;
                }

                if (!int.TryParse(numbers[0], out int row)
                    || !int.TryParse(numbers[1], out int col)
                    || row < 1 || row > size.Rows
                    || col < 1 || col > size.Cols)
                {
                    Console.WriteLine("Нужно ввести номер строки и столбца.\n");
                    continue;
                }

                if (revealed[--row, --col])
                {
                    Console.WriteLine("Эта карта уже перевернута.\n");
                    continue;
                }

                return (row, col);
            }
        }

        static void DisplayBoard(int[,] cards, bool[,] revealed)
        {
            for (int i = 0; i < cards.GetLength(0); i++)
            {
                for (int j = 0; j < cards.GetLength(1); j++)
                {
                    if (revealed[i, j])
                    {
                        Console.Write($"{cards[i, j]} ");
                    }
                    else
                    {
                        Console.Write("* ");
                    }
                }

                Console.WriteLine();
            }

            Console.WriteLine();
        }

        static bool IsEqual(int firstValue, int secondValue)
        {
            return firstValue == secondValue;
        }

        static bool IsWin(bool[,] revealed)
        {
            for (int i = 0; i < revealed.GetLength(0); i++)
            {
                for (int j = 0; j < revealed.GetLength(1); j++)
                {
                    if (!revealed[i, j])
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        static void Main()
        {
            (int Rows, int Cols) size = GetFieldSize();

            int[,] cards = InitializeCards(size.Rows, size.Cols);
            bool[,] revealed = new bool[size.Rows, size.Cols];

            do
            {
                DisplayBoard(cards, revealed);

                (int Row, int Col) firstCardPosition = GetPosition(size, revealed);
                revealed[firstCardPosition.Row, firstCardPosition.Col] = true;

                Console.Clear();

                DisplayBoard(cards, revealed);

                (int Row, int Col) secondCardPosition = GetPosition(size, revealed);
                revealed[secondCardPosition.Row, secondCardPosition.Col] = true;

                Console.Clear();

                if (!IsEqual(cards[firstCardPosition.Row, firstCardPosition.Col],
                    cards[secondCardPosition.Row, secondCardPosition.Col]))
                {
                    revealed[firstCardPosition.Row, firstCardPosition.Col] = false;
                    revealed[secondCardPosition.Row, secondCardPosition.Col] = false;
                }
            } while (!IsWin(revealed));

            DisplayBoard(cards, revealed);

            Console.WriteLine("\nВы победили!!!");
        }
    }
}