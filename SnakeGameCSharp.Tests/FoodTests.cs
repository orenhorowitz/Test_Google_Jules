using Microsoft.VisualStudio.TestTools.UnitTesting;
using SnakeGameCSharp;
using System.Collections.Generic;
using System.Linq;

namespace SnakeGameCSharp.Tests
{
    [TestClass]
    public class FoodTests
    {
        [TestMethod]
        public void TestFoodGeneration_AlmostFullBoard()
        {
            int boardWidth = 10;
            int boardHeight = 10;
            var snakeBody = new List<(int X, int Y)>();

            // Create a snake that occupies almost the entire board, leaving (0,0) empty
            for (int y = 0; y < boardHeight; y++)
            {
                for (int x = 0; x < boardWidth; x++)
                {
                    if (x == 0 && y == 0) continue; // Leave (0,0) empty
                    snakeBody.Add((x, y));
                }
            }

            var food = new Food(boardWidth, boardHeight, snakeBody);

            Assert.IsTrue(food.Position.X >= 0 && food.Position.X < boardWidth, "Food X position is out of bounds.");
            Assert.IsTrue(food.Position.Y >= 0 && food.Position.Y < boardHeight, "Food Y position is out of bounds.");

            // Check that food is not on the snake
            bool foodOnSnake = snakeBody.Any(segment => segment.X == food.Position.X && segment.Y == food.Position.Y);
            Assert.IsFalse(foodOnSnake, "Food spawned on the snake's body.");

            // In this specific setup, food must be at (0,0)
            Assert.AreEqual((0, 0), food.Position, "Food did not spawn in the only available spot.");
        }

        [TestMethod]
        public void TestFoodGeneration_EmptyBoard()
        {
            int boardWidth = 5;
            int boardHeight = 5;
            var snakeBody = new List<(int X, int Y)>(); // Empty snake body

            var food = new Food(boardWidth, boardHeight, snakeBody);

            Assert.IsTrue(food.Position.X >= 0 && food.Position.X < boardWidth, "Food X position is out of bounds on empty board.");
            Assert.IsTrue(food.Position.Y >= 0 && food.Position.Y < boardHeight, "Food Y position is out of bounds on empty board.");
        }
    }
}
