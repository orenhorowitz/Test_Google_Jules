using System;
using System.Collections.Generic;
using System.Linq;

namespace SnakeGameCSharp
{
    public class Food
    {
        public (int X, int Y) Position { get; private set; }
        private readonly int _boardWidth;
        private readonly int _boardHeight;
        private readonly Random _random = new Random();

        public Food(int boardWidth, int boardHeight, List<(int X, int Y)> snakeBody)
        {
            _boardWidth = boardWidth;
            _boardHeight = boardHeight;
            GenerateNewPosition(snakeBody);
        }

        public void GenerateNewPosition(List<(int X, int Y)> snakeBody)
        {
            while (true)
            {
                int x = _random.Next(0, _boardWidth);
                int y = _random.Next(0, _boardHeight);

                // Check if the new position is on the snake
                bool onSnake = false;
                foreach (var segment in snakeBody)
                {
                    if (segment.X == x && segment.Y == y)
                    {
                        onSnake = true;
                        break;
                    }
                }

                if (!onSnake)
                {
                    Position = (x, y);
                    break;
                }
            }
        }
    }
}
