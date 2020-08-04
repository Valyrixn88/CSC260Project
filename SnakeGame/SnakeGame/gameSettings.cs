﻿using SnakeGame.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame
{
    public enum directions
    {
        Left,
        Right,
        Up,
        Down,
    };
    class gameSettings
    {
        public static int Width { get; set; }
        public static int Height { get; set; }
        public static int Speed { get; set; }
        public static int Score { get; set; }
        public static int Points { get; set; }

        public static bool gameOver { get; set; }

        public static directions direction { get; set; }

        public gameSettings()
        {
            Width = 15;
            Height = 15;
            Speed = 20;
            Score = 0;
            Points = 1;
            gameOver = false;
            direction = directions.Down;
        }
    }
}
