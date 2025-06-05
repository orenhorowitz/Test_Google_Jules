import unittest
from snake_game import Snake, Food, GameBoard, UP, DOWN, LEFT, RIGHT

class TestSnake(unittest.TestCase):

    def test_snake_initialization(self):
        snake = Snake(start_x=5, start_y=5, initial_direction=RIGHT)
        self.assertEqual(snake.body[0], (5, 5))
        self.assertEqual(snake.direction, RIGHT)
        self.assertEqual(len(snake.body), 3) # Default length is 3

    def test_snake_move_right(self):
        snake = Snake(start_x=5, start_y=5, initial_direction=RIGHT)
        # Initial body: [(5,5), (4,5), (3,5)]
        initial_head = snake.body[0]
        snake.move()
        # New head: (6,5). New body: [(6,5), (5,5), (4,5)]
        self.assertEqual(snake.body[0], (initial_head[0] + 1, initial_head[1]))
        self.assertEqual(snake.body[1], initial_head) # Old head is now the second segment

    def test_snake_move_left(self):
        snake = Snake(start_x=5, start_y=5, initial_direction=LEFT)
        # Initial body: [(5,5), (6,5), (7,5)]
        initial_head = snake.body[0]
        snake.move()
        # New head: (4,5). New body: [(4,5), (5,5), (6,5)]
        self.assertEqual(snake.body[0], (initial_head[0] - 1, initial_head[1]))

    def test_snake_move_up(self):
        snake = Snake(start_x=5, start_y=5, initial_direction=UP)
        # Initial body: [(5,5), (5,6), (5,7)]
        initial_head = snake.body[0]
        snake.move()
        # New head: (5,4). New body: [(5,4), (5,5), (5,6)]
        self.assertEqual(snake.body[0], (initial_head[0], initial_head[1] - 1))

    def test_snake_move_down(self):
        snake = Snake(start_x=5, start_y=5, initial_direction=DOWN)
        # Initial body: [(5,5), (5,4), (5,3)]
        initial_head = snake.body[0]
        snake.move()
        # New head: (5,6). New body: [(5,6), (5,5), (5,4)]
        self.assertEqual(snake.body[0], (initial_head[0], initial_head[1] + 1))

    def test_snake_change_direction_valid(self):
        snake = Snake(start_x=5, start_y=5, initial_direction=RIGHT)
        snake.change_direction('w') # 'w' maps to UP
        self.assertEqual(snake.direction, UP)
        snake.change_direction('a') # 'a' maps to LEFT
        self.assertEqual(snake.direction, LEFT)

    def test_snake_change_direction_invalid_reverse(self):
        snake = Snake(start_x=5, start_y=5, initial_direction=RIGHT)
        snake.change_direction('a') # Try to change to LEFT (opposite of RIGHT)
        self.assertEqual(snake.direction, RIGHT) # Direction should not change

        snake = Snake(start_x=5, start_y=5, initial_direction=UP)
        snake.change_direction('s') # Try to change to DOWN (opposite of UP)
        self.assertEqual(snake.direction, UP)

    def test_snake_grow(self):
        snake = Snake(start_x=5, start_y=5, initial_direction=RIGHT)
        initial_len = len(snake.body)
        snake.grow()
        snake.move() # Move is when growth takes effect
        self.assertEqual(len(snake.body), initial_len + 1)

    def test_snake_collision_wall_left(self):
        # Head starts at (0,5), body is (0,5), (1,5), (2,5). Moves LEFT.
        snake = Snake(start_x=0, start_y=5, initial_direction=LEFT)
        snake.move() # Head becomes (-1,5)
        board_width = 10
        board_height = 10
        self.assertTrue(snake.check_collision(board_width, board_height))

    def test_snake_collision_wall_right(self):
        board_width = 10
        snake = Snake(start_x=board_width - 1, start_y=5, initial_direction=RIGHT)
        # Head starts at (9,5), body (9,5), (8,5), (7,5). Moves RIGHT.
        snake.move() # Head becomes (10,5)
        board_height = 10
        self.assertTrue(snake.check_collision(board_width, board_height))

    def test_snake_collision_wall_up(self):
        snake = Snake(start_x=5, start_y=0, initial_direction=UP)
        # Head starts at (5,0), body (5,0), (5,1), (5,2). Moves UP.
        snake.move() # Head becomes (5,-1)
        board_width = 10
        board_height = 10
        self.assertTrue(snake.check_collision(board_width, board_height))

    def test_snake_collision_wall_down(self):
        board_height = 10
        snake = Snake(start_x=5, start_y=board_height - 1, initial_direction=DOWN)
        # Head starts at (5,9), body (5,9), (5,8), (5,7). Moves DOWN.
        snake.move() # Head becomes (5,10)
        board_width = 10
        self.assertTrue(snake.check_collision(board_width, board_height))

    def test_snake_collision_self(self):
        # Setup a scenario where the snake will collide with itself
        # Body: H=(5,5), B1=(4,5), B2=(3,5)
        snake = Snake(start_x=5, start_y=5, initial_direction=RIGHT)

        # Force a self-colliding body structure:
        # Head at (5,5), then (6,5), (6,6), (5,6) - forms a small loop
        # If current direction is UP, next head will be (5,4) - no collision
        # If current direction is RIGHT, next head will be (6,5) - collision
        snake.body = [(5,5), (6,5), (6,6), (5,6)]
        snake.direction = RIGHT # Try to move into (6,5) where body[1] is

        # Actually, for self collision, the head must attempt to move into a segment *other* than the one immediately following it
        # if it's a short snake.
        # A more reliable setup for a length 4 snake:
        # H B1 B2 B3
        # (5,5) -> (6,5) -> (7,5) -> (7,6) -> (6,6) -> (5,6) -> (5,5) ...
        # Let snake be: [(5,5), (4,5), (3,5), (3,6)]
        # Change direction to UP: head (5,5) -> (5,4)
        # Change direction to RIGHT: head (5,5) -> (6,5)
        # Change direction to LEFT: head (5,5) -> (4,5) -> collision!

        snake.body = [(5,5), (4,5), (3,5), (3,6)] # Head at (5,5)
        snake.direction = LEFT # Next head will be (4,5)
        snake.move() # Head becomes (4,5), which is snake.body[1] (now snake.body[2] after insert)

        board_width = 20
        board_height = 20
        # After move: body is [(4,5), (5,5), (4,5), (3,5)]
        # Head (4,5) is in self.body[1:] which is [(5,5), (4,5), (3,5)]
        self.assertTrue(snake.check_collision(board_width, board_height))


class TestFood(unittest.TestCase):

    def test_food_generation(self):
        board_width = 10
        board_height = 10
        # Create a snake that occupies almost the entire board
        snake_body = []
        for r in range(board_height):
            for c in range(board_width):
                if (r,c) != (0,0): # Leave one spot for food
                    snake_body.append((c,r))

        food = Food(board_width, board_height, snake_body)
        self.assertTrue(0 <= food.position[0] < board_width)
        self.assertTrue(0 <= food.position[1] < board_height)
        self.assertNotIn(food.position, snake_body)
        self.assertEqual(food.position, (0,0)) # Should be the only spot left

    def test_food_generation_empty_board(self):
        board_width = 5
        board_height = 5
        snake_body = [] # No snake
        food = Food(board_width, board_height, snake_body)
        self.assertTrue(0 <= food.position[0] < board_width)
        self.assertTrue(0 <= food.position[1] < board_height)


if __name__ == '__main__':
    unittest.main()
