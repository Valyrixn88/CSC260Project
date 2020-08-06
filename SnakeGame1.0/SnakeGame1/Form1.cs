using SnakeGame1;
using SnakeGame1.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SnakeGame1
{
    public partial class Form1 : Form
    {
        private List<Snake> Snake = new List<Snake>();
        private Snake food = new Snake();
        public Form1()
        {
            InitializeComponent();

            new gameSettings();

            gameTimer.Interval = 1000 / gameSettings.Speed;
            gameTimer.Tick += updateScreen;
            gameTimer.Start();

            startGame();
        }

        private void updateScreen(object sender, EventArgs e)
        {
            if (gameSettings.gameOver == true)
            {
                if (Input.KeyPress(Keys.Enter))
                {
                    startGame();
                }
            }
            else
            {
                if (Input.KeyPress(Keys.Right) && gameSettings.direction != directions.Left)
                {
                    gameSettings.direction = directions.Right;
                }
                else if (Input.KeyPress(Keys.Left) && gameSettings.direction != directions.Right)
                {
                    gameSettings.direction = directions.Left;
                }
                else if (Input.KeyPress(Keys.Up) && gameSettings.direction != directions.Down)
                {
                    gameSettings.direction = directions.Up;
                }
                else if (Input.KeyPress(Keys.Down) && gameSettings.direction != directions.Up)
                {
                    gameSettings.direction = directions.Down;
                }

                moveSnake();
            }

            pbCanvas.Invalidate();
        }

        private void moveSnake()
        {
            for (int i = Snake.Count - 1; i >= 0; i++)
            {
                if (i == 0)
                {
                    switch (gameSettings.direction)
                    {
                        case directions.Right:
                            Snake[i].X++;
                            break;
                        case directions.Left:
                            Snake[i].X--;
                            break;
                        case directions.Down:
                            Snake[i].Y++;
                            break;
                        case directions.Up:
                            Snake[i].Y--;
                            break;
                    }

                    int maxXposition = pbCanvas.Size.Width / gameSettings.Width;
                    int maxYposition = pbCanvas.Size.Height / gameSettings.Height;

                    if (Snake[i].X < 0 || Snake[i].Y < 0 || Snake[i].X > maxXposition || Snake[i].Y > maxYposition)
                    {
                        dead();
                    }

                    for (int a = 1; a < Snake.Count; a++)
                    {
                        if (Snake[i].X == Snake[a].X && Snake[i].Y == Snake[a].Y)
                        {
                            dead();
                        }
                    }

                    if (Snake[0].X == food.X && Snake[0].Y == food.Y)
                    {
                        grow();
                    }
                    else
                    {
                        Snake[i].X = Snake[i - 1].X;
                        Snake[i].Y = Snake[i - 1].Y;
                    }
                }
            }


            private void updateGraphics(object sender, PaintEventArgs e)
            {
                if (gameSettings.gameOver == false)
                {
                    Brush snakeColour;

                    for (int i = 0; i < Snake.Count; i++)
                    {
                        if (i == 0)
                        {
                            snakeColour = Brushes.Black;
                        }
                        else
                        {
                            snakeColour = Brushes.Green;
                        }

                        canvas.FillEllipse(snakeColour, new Rectangle(Snake[i].X * gameSettings.Width, Snake[i].Y * gameSettings.Height, gameSettings.Width, gameSettings.Height));

                        canvas.FillEllipse(Brushes.Red, food.X * gameSettings.Width, food.Y * gameSettings.Height, gameSettings.Width, gameSettings.Height));
        
                else
                {
                    string gameOver = "Game Over \n" + "Final Score is " + gameSettings.Score + "\n Press enter to Restart \n";
                    label3.Text = gameOver;
                    label3.Visible = true;
                }
            }
            private void startGame()
            {
                 label3.Visible = false;
                 new gameSettings();
                Snake.Clear();
                Snake head = new Snake { X = 10, Y = 5 };
                Snake.Add(head);

                label2.Text = gameSettings.Score.ToString();
                generateFood();
            }
            private void generateFood()
            {
                int maxXposition = pbCanvas.Size.Width / gameSettings.Width;
                int maxYposition = pbCanvas.Size.Height / gameSettings.Height;

                Random random = new Random();
                food = new Snake { X = random.Next(Object, maxXposition), Y = random.Next(0, maxYposition) };
            }

            private void eat()
            {
            Snake body = new Snake
                {
                    X = Snake[Snake.Count - 1].X,
                    Y = Snake[Snake.Count - 1].Y
                };

                Snake.Add(body);
                gameSettings.Score += gameSettings.Points;
                label2.Text = gameSettings.Score.ToString();
                generateFood();
            }

            private void dead()
            {
                gameSettings.gameOver = true;
            }

        private void KeyIsDown(object sender, KeyEventArgs e)
        {
            Input.changeState(e.KeyCode, true);
        }

        private void KeyIsUp(object sender, KeyEventArgs e)
        {
            Input.changeState(e.KeyCode, false);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
