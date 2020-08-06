using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace SnakeGame
{
    class Program
    {
        const int SCREEN_MULT = 5;
        public const int SCREEN_W = 12 * SCREEN_MULT;
        public const int SCREEN_H = 5 * SCREEN_MULT;

        static void Main(string[] args)
        {
            var s = new Snake();
            var f = new Food();
            bool escape = false;

            InitGame(s, f);
            escape = Welcome(escape);

            while (!escape)
            {
                ResetGame(s, f);
                escape = PlayGame(s, f, escape);
                if (!escape)
                {
                    escape = DoGameOver(s, escape);
                }
            }
        }

        static bool Welcome(bool escape)
        {
            ConsoleKeyInfo keyInfo;

            string[] Instructions = { "Welcome to the snake game", "Your goal is to collect the red fruit", "Use the arrow keys to move around", "Press esc to exit", "Press any key to begin" };

            for (int i = 0; i < Instructions.Count(); i++)
            {
                Console.SetCursorPosition((SCREEN_W / 2) - ((Instructions[i].Length / 2)), (SCREEN_H / 2) + i - (Instructions.Count() / 2));
                Console.WriteLine(Instructions[i]);
            }

            if ((keyInfo = Console.ReadKey(true)).Key == ConsoleKey.Escape)
            {
                return true;
            }

            return false;
        }

        static void InitGame(Snake s, Food f)
        {
            Console.CursorVisible = false;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WindowHeight = SCREEN_H;
            Console.WindowWidth = SCREEN_W;
        }

        static bool PlayGame(Snake s, Food f, bool escape)
        {
            ConsoleKeyInfo keyInfo;
            Snake.ShowScore(s);

            while (true)
            {
                s.doGrow = false;

                Thread.Sleep(Convert.ToInt16(100 / s.speed));
                Console.Clear();
                if (Console.KeyAvailable == true)
                {
                    keyInfo = Console.ReadKey(true);
                    switch (keyInfo.Key)
                    {
                        case ConsoleKey.UpArrow:
                            Snake.SetDir(0, -1, s);
                            break;
                        case ConsoleKey.DownArrow:
                            Snake.SetDir(0, 1, s);
                            break;
                        case ConsoleKey.RightArrow:
                            Snake.SetDir(1, 0, s);
                            break;
                        case ConsoleKey.LeftArrow:
                            Snake.SetDir(-1, 0, s);
                            break;
                        case ConsoleKey.Escape:
                            escape = true;
                            return escape;
                    }
                }
                Snake.Eat(s, f);
                Snake.Update(s);
                if (s.dead)
                {
                    break;
                }
                Snake.ShowScore(s);
                Food.Show(f);
                Snake.Show(s);
            }
            return false;
        }

        static bool DoGameOver(Snake s, bool escape)
        {
            string[] Instructions = { "Game  Over", "Score: " + s.score };
            ConsoleKeyInfo keyInfo;

            for (int i = 0; i < Instructions.Count(); i++)
            {
                Console.SetCursorPosition((SCREEN_W / 2) - ((Instructions[i].Length / 2)), (SCREEN_H / 2) + i - (Instructions.Count() / 2));
                Console.WriteLine(Instructions[i]);
            }

            if (s.score > s.highScore)
            {
                s.highScore = s.score;
                Console.SetCursorPosition((SCREEN_W / 2) - 7, (SCREEN_H / 2) + 2);
                Console.WriteLine("NEW HIGH SCORE!!!");
            }

            Console.SetCursorPosition((SCREEN_W / 2) - 6, (SCREEN_H / 2) + 3);
            Console.WriteLine("Highscore: " + s.highScore);
            Thread.Sleep(3000);
            if (!escape)
            {
                Console.Clear();
                Console.SetCursorPosition((SCREEN_W / 2) - 8, (SCREEN_H / 2) - 1);
                Console.WriteLine("Press esc to exit");
                Console.SetCursorPosition((SCREEN_W / 2) - 12, (SCREEN_H / 2) + 1);
                Console.WriteLine("Or press any key play again");
                if ((keyInfo = Console.ReadKey(true)).Key == ConsoleKey.Escape)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        static void ResetGame(Snake s, Food f)
        {
            Food.NewPosition(f, s);
            Snake.InitSnake(s);
        }
    }








    class Snake
    {
        const double SPEED_INCREMENT = 0.3;
        const int START_LENGTH = 10;
        const char SNAKE_CHAR = '@';
        const ConsoleColor SNAKE_COLOUR = ConsoleColor.Green;

        public int x;
        public int y;
        public int xDir;
        public int yDir;
        public int score;
        public int highScore = 0;
        public double speed;
        public bool doGrow;
        public bool dead;
        public List<int> xPositions = new List<int>();
        public List<int> yPositions = new List<int>();

        public static void InitSnake(Snake s)
        {
            s.x = 9;
            s.y = 3;
            s.xDir = 1;
            s.yDir = 0;
            s.score = 0;
            s.speed = 1.2;
            s.doGrow = false;
            s.dead = false;
            s.xPositions.Clear();
            s.yPositions.Clear();

            for (int i = START_LENGTH - 1; i >= 0; i--)
            {
                s.xPositions.Add(s.x - i);
                s.yPositions.Add(s.y);
            }
        }

        public static void SetDir(int x, int y, Snake s)
        {
            if (s.xDir != -x && s.yDir != -y)
            {
                s.xDir = x;
                s.yDir = y;
            }
        }

        public static void Update(Snake s)
        {
            s.x = s.x + s.xDir;
            s.y = s.y + s.yDir;

            if (IsGameOver(s))
            {
                s.dead = true;
            }

            s.xPositions.Add(s.x);
            s.yPositions.Add(s.y);
            if (!s.doGrow)
            {
                s.xPositions.RemoveAt(0);
                s.yPositions.RemoveAt(0);
            }
        }

        public static void Eat(Snake s, Food f)
        {
            if (s.x == f.x && s.y == f.y)
            {
                s.speed = s.speed + SPEED_INCREMENT;
                s.score++;
                Food.NewPosition(f, s);
                s.doGrow = true;
            }
        }

        public static void Show(Snake s)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            for (int i = 0; i < s.xPositions.Count(); i++)
            {
                Console.SetCursorPosition(s.xPositions[i], s.yPositions[i]);
                Console.Write(SNAKE_CHAR);
            }
        }

        public static void ShowScore(Snake s)
        {
            Console.ForegroundColor = SNAKE_COLOUR;
            Console.SetCursorPosition(0, 0);
            Console.WriteLine("Score: " + s.score);
        }

        static bool IsGameOver(Snake s)
        {
            if (s.x < 0 || s.x > Program.SCREEN_W - 1 || s.y < 0 || s.y > Program.SCREEN_H - 1)
            {
                return true;
            }
            for (int i = 0; i < s.xPositions.Count(); i++)
            {
                if (s.xPositions[i] == s.x && s.yPositions[i] == s.y)
                {
                    return true;
                }
            }
            return false;
        }
    }

    class Food
    {
        const ConsoleColor FOOD_COLOUR = ConsoleColor.Red;
        const ConsoleColor BACK_COLOUR = ConsoleColor.Black;
        const int foodPosBuffer = 2;

        public int x = 10;
        public int y = 10;

        public static void Show(Food f)
        {
            Console.SetCursorPosition(f.x, f.y);
            Console.BackgroundColor = FOOD_COLOUR;
            Console.WriteLine(" ");
            Console.BackgroundColor = BACK_COLOUR;
        }

        public static void NewPosition(Food f, Snake s)
        {
            int newX;
            int newY;
            Random pos = new Random();

            do
            {
                newX = pos.Next(foodPosBuffer, Program.SCREEN_W - foodPosBuffer);
                newY = pos.Next(foodPosBuffer, Program.SCREEN_H - foodPosBuffer);
            } while (ValidPos(f, s, newX, newY) == false);

            f.x = newX;
            f.y = newY;
        }

        static bool ValidPos(Food f, Snake s, int newX, int newY)
        {
            if (newX == f.x || newY == f.y)
            {
                return false;
            }
            if (s.xPositions.Contains(newX) || s.yPositions.Contains(newY))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

    }
}