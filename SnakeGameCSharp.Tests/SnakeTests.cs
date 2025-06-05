using Microsoft.VisualStudio.TestTools.UnitTesting;
using SnakeGameCSharp; // Assuming your main game classes are in this namespace
using System.Linq;
using System.Collections.Generic;
using System; // For ConsoleKey

namespace SnakeGameCSharp.Tests
{
    [TestClass]
    public class SnakeTests
    {
        [TestMethod]
        public void TestSnakeInitialization()
        {
            var snake = new Snake(startX: 5, startY: 5, initialDirection: Direction.RIGHT);
            Assert.AreEqual((5, 5), snake.Body.First(), "Head position is incorrect.");
            Assert.AreEqual(Direction.RIGHT, snake.CurrentDirection, "Initial direction is incorrect.");
            Assert.AreEqual(3, snake.Body.Count, "Initial snake length should be 3.");
        }

        [TestMethod]
        public void TestSnakeMoveRight()
        {
            var snake = new Snake(startX: 5, startY: 5, initialDirection: Direction.RIGHT);
            // Initial body: [(5,5), (4,5), (3,5)]
            var initialHead = snake.Body.First();
            snake.Move();
            // New head: (6,5). New body: [(6,5), (5,5), (4,5)]
            Assert.AreEqual((initialHead.X + 1, initialHead.Y), snake.Body.First(), "Snake did not move right correctly.");
            Assert.AreEqual(initialHead, snake.Body[1], "Old head is not the second segment.");
        }

        [TestMethod]
        public void TestSnakeMoveLeft()
        {
            var snake = new Snake(startX: 5, startY: 5, initialDirection: Direction.LEFT);
            // Initial body: [(5,5), (6,5), (7,5)]
            var initialHead = snake.Body.First();
            snake.Move();
            // New head: (4,5). New body: [(4,5), (5,5), (6,5)]
            Assert.AreEqual((initialHead.X - 1, initialHead.Y), snake.Body.First(), "Snake did not move left correctly.");
        }

        [TestMethod]
        public void TestSnakeMoveUp()
        {
            var snake = new Snake(startX: 5, startY: 5, initialDirection: Direction.UP);
            // Initial body: [(5,5), (5,6), (5,7)]
            var initialHead = snake.Body.First();
            snake.Move();
            // New head: (5,4). New body: [(5,4), (5,5), (5,6)]
            Assert.AreEqual((initialHead.X, initialHead.Y - 1), snake.Body.First(), "Snake did not move up correctly.");
        }

        [TestMethod]
        public void TestSnakeMoveDown()
        {
            var snake = new Snake(startX: 5, startY: 5, initialDirection: Direction.DOWN);
            // Initial body: [(5,5), (5,4), (5,3)]
            var initialHead = snake.Body.First();
            snake.Move();
            // New head: (5,6). New body: [(5,6), (5,5), (5,4)]
            Assert.AreEqual((initialHead.X, initialHead.Y + 1), snake.Body.First(), "Snake did not move down correctly.");
        }

        [TestMethod]
        public void TestSnakeChangeDirectionValid()
        {
            var snake = new Snake(startX: 5, startY: 5, initialDirection: Direction.RIGHT);
            snake.ChangeDirection(ConsoleKey.W); // W maps to UP
            Assert.AreEqual(Direction.UP, snake.CurrentDirection, "Direction should change to UP.");
            snake.ChangeDirection(ConsoleKey.A); // A maps to LEFT
            Assert.AreEqual(Direction.LEFT, snake.CurrentDirection, "Direction should change to LEFT.");
        }

        [TestMethod]
        public void TestSnakeChangeDirectionInvalidReverse()
        {
            var snakeRight = new Snake(startX: 5, startY: 5, initialDirection: Direction.RIGHT);
            snakeRight.ChangeDirection(ConsoleKey.A); // Try to change to LEFT (opposite of RIGHT)
            Assert.AreEqual(Direction.RIGHT, snakeRight.CurrentDirection, "Snake should not reverse direction from RIGHT to LEFT.");

            var snakeUp = new Snake(startX: 5, startY: 5, initialDirection: Direction.UP);
            snakeUp.ChangeDirection(ConsoleKey.S); // Try to change to DOWN (opposite of UP)
            Assert.AreEqual(Direction.UP, snakeUp.CurrentDirection, "Snake should not reverse direction from UP to DOWN.");
        }

        [TestMethod]
        public void TestSnakeGrow()
        {
            var snake = new Snake(startX: 5, startY: 5, initialDirection: Direction.RIGHT);
            int initialLen = snake.Body.Count;
            snake.Grow();
            snake.Move(); // Growth takes effect upon moving
            Assert.AreEqual(initialLen + 1, snake.Body.Count, "Snake length should increase by 1 after growing.");
        }

        [TestMethod]
        public void TestSnakeCollisionWallLeft()
        {
            var snake = new Snake(startX: 0, startY: 5, initialDirection: Direction.LEFT);
            // Initial head (0,5), body: [(0,5), (1,5), (2,5)]
            snake.Move(); // Head becomes (-1,5)
            Assert.IsTrue(snake.CheckCollision(boardWidth: 10, boardHeight: 10), "Should detect collision with left wall.");
        }

        [TestMethod]
        public void TestSnakeCollisionWallRight()
        {
            int boardWidth = 10;
            var snake = new Snake(startX: boardWidth - 1, startY: 5, initialDirection: Direction.RIGHT);
            // Initial head (9,5), body: [(9,5), (8,5), (7,5)]
            snake.Move(); // Head becomes (10,5)
            Assert.IsTrue(snake.CheckCollision(boardWidth: boardWidth, boardHeight: 10), "Should detect collision with right wall.");
        }

        [TestMethod]
        public void TestSnakeCollisionWallUp()
        {
            var snake = new Snake(startX: 5, startY: 0, initialDirection: Direction.UP);
            // Initial head (5,0), body: [(5,0), (5,1), (5,2)]
            snake.Move(); // Head becomes (5,-1)
            Assert.IsTrue(snake.CheckCollision(boardWidth: 10, boardHeight: 10), "Should detect collision with top wall.");
        }

        [TestMethod]
        public void TestSnakeCollisionWallDown()
        {
            int boardHeight = 10;
            var snake = new Snake(startX: 5, startY: boardHeight - 1, initialDirection: Direction.DOWN);
            // Initial head (5,9), body: [(5,9), (5,8), (5,7)]
            snake.Move(); // Head becomes (5,10)
            Assert.IsTrue(snake.CheckCollision(boardWidth: 10, boardHeight: boardHeight), "Should detect collision with bottom wall.");
        }

        [TestMethod]
        public void TestSnakeCollisionSelf()
        {
            // Body: H=(2,0), (1,0), (0,0). Make it turn and collide with itself.
            // Current: (2,0)R -> (1,0) -> (0,0)
            // Move:    (3,0)R -> (2,0) -> (1,0)
            // Change Dir: UP
            // Move:    (3,-1)U -> (3,0) -> (2,0)
            // Change Dir: LEFT
            // Move:    (2,-1)L -> (3,-1) -> (3,0)
            // Change Dir: DOWN
            // Move:    (2,0)D -> (2,-1) -> (3,-1)  COLLISION with Body[2] after head insertion

            var snake = new Snake(startX: 2, startY: 0, initialDirection: Direction.RIGHT); // Body: (2,0), (1,0), (0,0)

            // Force a situation for self-collision more directly for a default length 3 snake
            // Snake: [(2,0), (1,0), (0,0)] Direction: RIGHT
            // Move 1: [(3,0), (2,0), (1,0)] Direction: RIGHT
            snake.Move();
            // Change Direction to UP
            snake.ChangeDirection(ConsoleKey.W); // Direction: UP
            // Move 2: [(3,-1), (3,0), (2,0)] Direction: UP
            snake.Move();
             // Change Direction to LEFT
            snake.ChangeDirection(ConsoleKey.A); // Direction: LEFT
            // Move 3: [(2,-1), (3,-1), (3,0)] Direction: LEFT
            snake.Move();
            // Change Direction to DOWN
            snake.ChangeDirection(ConsoleKey.S); // Direction: DOWN
            // Move 4: Head will be (2,0). Body before move: [(2,-1), (3,-1), (3,0)]
            // After new head: [(2,0), (2,-1), (3,-1), (3,0)]. The new head (2,0) collides with old Body[2] (which is (3,0) before move, (2,0) in Python version due to different init)
            // Let's simplify the Python's self-collision test logic directly:
            // snake.body = [(5,5), (4,5), (3,5), (3,6)]
            // snake.direction = LEFT
            // snake.move() -> new head (4,5) collides with (4,5) in body

            var customSnake = new Snake(startX: 0, startY: 0, initialDirection: Direction.RIGHT); // Dummy, we overwrite body
            customSnake.Body.Clear();
            customSnake.Body.Add((5,5)); // Head
            customSnake.Body.Add((4,5));
            customSnake.Body.Add((3,5));
            customSnake.Body.Add((3,6)); // Tail segment that will cause issue

            customSnake.ChangeDirection(ConsoleKey.A); // Change to LEFT, original direction was RIGHT in constructor, but it doesn't matter as we set it
            // Ensure CurrentDirection is LEFT for the test
            // We need to set CurrentDirection directly if ChangeDirection doesn't allow it due to opposite rule
            // The Snake class's ChangeDirection method has protection. So, let's initialize with a neutral direction first or set manually.
            // Re-initialize snake for a cleaner setup for this specific test case
            var selfCollideSnake = new Snake(startX: 5, startY: 5, initialDirection: Direction.DOWN); // Start DOWN
            selfCollideSnake.Body.Clear(); // Manually set up body for collision
            selfCollideSnake.Body.Add((5,5)); // Head
            selfCollideSnake.Body.Add((4,5)); // Segment 1
            selfCollideSnake.Body.Add((3,5)); // Segment 2
            selfCollideSnake.Body.Add((3,6)); // Segment 3
            // Current body: [(5,5), (4,5), (3,5), (3,6)]

            selfCollideSnake.ChangeDirection(ConsoleKey.A); // Request LEFT. Current is DOWN. Valid change.
            // Now CurrentDirection is LEFT.
            // Next move, head will be (4,5).

            selfCollideSnake.Move(); // Head becomes (4,5). Body: [(4,5), (5,5), (4,5), (3,5)] (approx)

            Assert.IsTrue(selfCollideSnake.CheckCollision(boardWidth: 20, boardHeight: 20), "Should detect self-collision.");
        }
    }
}
