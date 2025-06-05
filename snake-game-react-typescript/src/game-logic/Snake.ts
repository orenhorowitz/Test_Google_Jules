import { Coordinates, Direction } from "./types";

export class Snake {
  body: Coordinates[];
  direction: Direction;

  constructor(initialPosition: Coordinates, initialDirection: Direction) {
    this.body = [initialPosition];
    this.direction = initialDirection;
  }

  move() {
    const head = { ...this.body[0] }; // Copy the head

    switch (this.direction) {
      case "UP":
        head.y -= 1;
        break;
      case "DOWN":
        head.y += 1;
        break;
      case "LEFT":
        head.x -= 1;
        break;
      case "RIGHT":
        head.x += 1;
        break;
    }

    this.body.unshift(head); // Add new head
    this.body.pop(); // Remove tail
  }

  grow() {
    const head = { ...this.body[0] }; // Copy the head, it will be the new segment
     // The new segment is added at the head's current position,
     // it will be correctly positioned after the next move.
    this.body.unshift(head);
  }

  checkCollision(boardWidth: number, boardHeight: number, selfCollisionOnly: boolean = false): boolean {
    const head = this.body[0];

    if (!selfCollisionOnly) {
        // Wall collision
        if (head.x < 0 || head.x >= boardWidth || head.y < 0 || head.y >= boardHeight) {
            return true;
        }
    }

    // Self-collision
    for (let i = 1; i < this.body.length; i++) {
      if (head.x === this.body[i].x && head.y === this.body[i].y) {
        return true;
      }
    }

    return false;
  }

  changeDirection(newDirection: Direction) {
    // Prevent the snake from reversing directly onto itself
    if (
      (this.direction === "UP" && newDirection === "DOWN") ||
      (this.direction === "DOWN" && newDirection === "UP") ||
      (this.direction === "LEFT" && newDirection === "RIGHT") ||
      (this.direction === "RIGHT" && newDirection === "LEFT")
    ) {
      return;
    }
    this.direction = newDirection;
  }
}
