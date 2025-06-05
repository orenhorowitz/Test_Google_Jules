import { Coordinates } from "./types";
import { Snake } from "./Snake"; // Import Snake to check its body for food placement

export class Food {
  position: Coordinates;

  constructor(boardWidth: number, boardHeight: number, snakeBody: Coordinates[]) {
    this.position = this.generateNewPosition(boardWidth, boardHeight, snakeBody);
  }

  generateNewPosition(boardWidth: number, boardHeight: number, snakeBody: Coordinates[]): Coordinates {
    let newPosition: Coordinates;
    let collisionWithSnake: boolean;

    do {
      newPosition = {
        x: Math.floor(Math.random() * boardWidth),
        y: Math.floor(Math.random() * boardHeight),
      };

      collisionWithSnake = snakeBody.some(
        (segment) => segment.x === newPosition.x && segment.y === newPosition.y
      );
    } while (collisionWithSnake);

    this.position = newPosition;
    return newPosition;
  }
}
