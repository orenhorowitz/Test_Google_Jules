using System;
using System.Collections.Generic;
using System.Threading; // Required for Thread.Sleep

namespace SnakeGameCSharp
{
    class Program
    {
        static void Main(string[] args)
        {
            int boardWidth = 20;
            int boardHeight = 10;

            GameBoard gameBoard = new GameBoard(boardWidth, boardHeight);
            Snake snake = new Snake(startX: boardWidth / 2, startY: boardHeight / 2, initialDirection: Direction.RIGHT);
            Food food = new Food(boardWidth, boardHeight, snake.Body);

            int score = 0;
            bool gameOver = false;
            ConsoleKeyInfo keyInfo;

            // Initial draw
            DrawGameElements(gameBoard, snake, food);
            Console.WriteLine($"Score: {score}");

            while (!gameOver)
            {
                // --- Input Handling ---
                if (Console.KeyAvailable) // Check if a key has been pressed
                {
                    keyInfo = Console.ReadKey(true); // Read key without displaying it
                    snake.ChangeDirection(keyInfo.Key);
                }

                // --- Update ---
                snake.Move();

                // --- Collision Check ---
                if (snake.CheckCollision(boardWidth, boardHeight))
                {
                    gameOver = true;
                    break;
                }

                // --- Food Consumption ---
                (int headX, int headY) = snake.Body[0];
                if (headX == food.Position.X && headY == food.Position.Y)
                {
                    score++;
                    snake.Grow();
                    food.GenerateNewPosition(snake.Body);
                }

                // --- Display ---
                DrawGameElements(gameBoard, snake, food);
                Console.WriteLine($"Score: {score}");

                // --- Delay ---
                Thread.Sleep(200); // Milliseconds
            }

            // --- Game Over Message ---
            Console.SetCursorPosition(0, boardHeight + 1); // Move cursor below the board
            Console.WriteLine("Game Over!");
            Console.WriteLine($"Final Score: {score}");
        }

        static void DrawGameElements(GameBoard gameBoard, Snake snake, Food food)
        {
            Console.Clear(); // Clear console for fresh draw (can cause flicker, alternative is SetCursorPosition)
            // Or, position cursor at top-left for redrawing: Console.SetCursorPosition(0, 0);

            gameBoard.Clear(); // Clear internal board representation

            // Draw snake
            for (int i = 0; i < snake.Body.Count; i++)
            {
                var segment = snake.Body[i];
                char charToDraw = (i == 0) ? 'S' : 's'; // Head is 'S', body is 's'
                gameBoard.DrawElement(segment.X, segment.Y, charToDraw);
            }

            // Draw food
            gameBoard.DrawElement(food.Position.X, food.Position.Y, 'F');

            gameBoard.Display();
        }
    }
}
