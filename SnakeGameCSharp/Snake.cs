using System;
using System.Collections.Generic;
using System.Linq;

namespace SnakeGameCSharp
{
    public enum Direction
    {
        UP,
        DOWN,
        LEFT,
        RIGHT
    }

    public class Snake
    {
        public List<(int X, int Y)> Body { get; private set; }
        public Direction CurrentDirection { get; private set; }
        private bool _justAte;

        public Snake(int startX, int startY, Direction initialDirection)
        {
            CurrentDirection = initialDirection;
            _justAte = false;
            InitializeBody(startX, startY, initialDirection);
        }

        private void InitializeBody(int startX, int startY, Direction initialDirection)
        {
            Body = new List<(int X, int Y)>();
            Body.Add((startX, startY)); // Head

            // Initial body based on direction to make a 3-segment snake
            switch (initialDirection)
            {
                case Direction.RIGHT:
                    Body.Add((startX - 1, startY));
                    Body.Add((startX - 2, startY));
                    break;
                case Direction.LEFT:
                    Body.Add((startX + 1, startY));
                    Body.Add((startX + 2, startY));
                    break;
                case Direction.UP:
                    Body.Add((startX, startY + 1));
                    Body.Add((startX, startY + 2));
                    break;
                case Direction.DOWN:
                    Body.Add((startX, startY - 1));
                    Body.Add((startX, startY - 2));
                    break;
            }
        }

        public void Move()
        {
            (int headX, int headY) = Body[0];
            (int newHeadX, int newHeadY) = (headX, headY);

            switch (CurrentDirection)
            {
                case Direction.UP:
                    newHeadY--;
                    break;
                case Direction.DOWN:
                    newHeadY++;
                    break;
                case Direction.LEFT:
                    newHeadX--;
                    break;
                case Direction.RIGHT:
                    newHeadX++;
                    break;
            }

            Body.Insert(0, (newHeadX, newHeadY));

            if (_justAte)
            {
                _justAte = false;
            }
            else
            {
                if (Body.Count > 1) // Ensure there's a tail to remove
                {
                    Body.RemoveAt(Body.Count - 1);
                }
            }
        }

        public void Grow()
        {
            _justAte = true;
        }

        public bool CheckCollision(int boardWidth, int boardHeight)
        {
            (int headX, int headY) = Body[0];

            // Wall collision
            if (headX < 0 || headX >= boardWidth || headY < 0 || headY >= boardHeight)
            {
                return true;
            }

            // Self-collision
            for (int i = 1; i < Body.Count; i++)
            {
                if (Body[i].X == headX && Body[i].Y == headY)
                {
                    return true;
                }
            }
            return false;
        }

        public void ChangeDirection(ConsoleKey key)
        {
            Direction newDirection = CurrentDirection;

            switch (key)
            {
                case ConsoleKey.W:
                case ConsoleKey.UpArrow:
                    newDirection = Direction.UP;
                    break;
                case ConsoleKey.S:
                case ConsoleKey.DownArrow:
                    newDirection = Direction.DOWN;
                    break;
                case ConsoleKey.A:
                case ConsoleKey.LeftArrow:
                    newDirection = Direction.LEFT;
                    break;
                case ConsoleKey.D:
                case ConsoleKey.RightArrow:
                    newDirection = Direction.RIGHT;
                    break;
                default:
                    return; // No change for other keys
            }

            // Prevent immediate reversal
            if (newDirection == Direction.UP && CurrentDirection != Direction.DOWN)
                CurrentDirection = newDirection;
            else if (newDirection == Direction.DOWN && CurrentDirection != Direction.UP)
                CurrentDirection = newDirection;
            else if (newDirection == Direction.LEFT && CurrentDirection != Direction.RIGHT)
                CurrentDirection = newDirection;
            else if (newDirection == Direction.RIGHT && CurrentDirection != Direction.LEFT)
                CurrentDirection = newDirection;
        }
    }
}
