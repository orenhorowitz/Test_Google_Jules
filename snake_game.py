import random
import time

# Direction constants
UP = "UP"
DOWN = "DOWN"
LEFT = "LEFT"
RIGHT = "RIGHT"

class GameBoard:
    def __init__(self, width, height):
        self.width = width
        self.height = height
        self.board = [[' ' for _ in range(width)] for _ in range(height)]

    def display(self):
        for row in self.board:
            print(''.join(row))

    def clear(self):
        self.board = [[' ' for _ in range(self.width)] for _ in range(self.height)]

    def draw_element(self, x, y, char):
        if 0 <= y < self.height and 0 <= x < self.width:
            self.board[y][x] = char

class Snake:
    def __init__(self, start_x, start_y, initial_direction=RIGHT):
        self.body = [(start_x, start_y)]
        # Initial body based on direction
        if initial_direction == RIGHT:
            self.body = [(start_x, start_y), (start_x - 1, start_y), (start_x - 2, start_y)]
        elif initial_direction == LEFT:
            self.body = [(start_x, start_y), (start_x + 1, start_y), (start_x + 2, start_y)]
        elif initial_direction == UP:
            self.body = [(start_x, start_y), (start_x, start_y + 1), (start_x, start_y + 2)]
        elif initial_direction == DOWN:
            self.body = [(start_x, start_y), (start_x, start_y - 1), (start_x, start_y - 2)]

        self.direction = initial_direction
        self.just_ate = False

    def move(self):
        head_x, head_y = self.body[0]

        if self.direction == UP:
            new_head = (head_x, head_y - 1)
        elif self.direction == DOWN:
            new_head = (head_x, head_y + 1)
        elif self.direction == LEFT:
            new_head = (head_x - 1, head_y)
        elif self.direction == RIGHT:
            new_head = (head_x + 1, head_y)

        self.body.insert(0, new_head)

        if self.just_ate:
            self.just_ate = False
        else:
            self.body.pop() # Remove tail

    def grow(self):
        self.just_ate = True

    def check_collision(self, board_width, board_height):
        head_x, head_y = self.body[0]

        # Wall collision
        if not (0 <= head_x < board_width and 0 <= head_y < board_height):
            return True

        # Self-collision
        if (head_x, head_y) in self.body[1:]:
            return True

        return False

    def change_direction(self, new_direction_input):
        # Mapping from input characters to direction constants
        input_to_direction = {
            'w': UP,
            's': DOWN,
            'a': LEFT,
            'd': RIGHT
        }

        new_direction = input_to_direction.get(new_direction_input.lower())

        if not new_direction: # Invalid input
            return

        if new_direction == UP and self.direction != DOWN:
            self.direction = new_direction
        elif new_direction == DOWN and self.direction != UP:
            self.direction = new_direction
        elif new_direction == LEFT and self.direction != RIGHT:
            self.direction = new_direction
        elif new_direction == RIGHT and self.direction != LEFT:
            self.direction = new_direction

class Food:
    def __init__(self, board_width, board_height, snake_body):
        self.board_width = board_width
        self.board_height = board_height
        self._generate_new_position(snake_body)

    def _generate_new_position(self, snake_body):
        while True:
            x = random.randint(0, self.board_width - 1)
            y = random.randint(0, self.board_height - 1)
            if (x, y) not in snake_body:
                self.position = (x, y)
                break

def game_loop():
    board_width = 20
    board_height = 10

    game_board = GameBoard(board_width, board_height)
    snake = Snake(start_x=board_width // 2, start_y=board_height // 2, initial_direction=RIGHT)
    food = Food(board_width, board_height, snake.body)

    score = 0
    game_over = False

    while not game_over:
        raw_input = input("Enter direction (w/a/s/d) then Enter: ")
        if raw_input:
            chosen_char = raw_input[0]
            snake.change_direction(chosen_char)

        snake.move()

        if snake.check_collision(board_width, board_height):
            game_over = True
            break

        head_x, head_y = snake.body[0]
        if (head_x, head_y) == food.position:
            score += 1
            snake.grow()
            food._generate_new_position(snake.body) # Pass current snake body

        game_board.clear()

        # Draw snake
        for i, (x, y) in enumerate(snake.body):
            char_to_draw = 'S' if i == 0 else 's'
            game_board.draw_element(x, y, char_to_draw)

        # Draw food
        game_board.draw_element(food.position[0], food.position[1], 'F')

        game_board.display()
        print(f"Score: {score}")

        time.sleep(0.2)

    print("Game Over!")
    print(f"Final Score: {score}")

if __name__ == '__main__':
    game_loop()
