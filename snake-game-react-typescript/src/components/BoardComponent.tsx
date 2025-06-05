import React from 'react';
import { GameBoard } from '../game-logic/GameBoard';
import { Snake } from '../game-logic/Snake';
import { Food } from '../game-logic/Food';
import { Coordinates } from '../game-logic/types';

interface BoardComponentProps {
  board: GameBoard;
  snake: Snake;
  food: Food;
}

const BoardComponent: React.FC<BoardComponentProps> = ({ board, snake, food }) => {
  const cells = [];

  for (let y = 0; y < board.height; y++) {
    for (let x = 0; x < board.width; x++) {
      let cellType = 'empty';
      const currentPos: Coordinates = { x, y };

      // Check for snake body
      if (snake.body.some(segment => segment.x === x && segment.y === y)) {
        cellType = 'snake';
      }
      // Check for snake head
      if (snake.body[0].x === x && snake.body[0].y === y) {
        cellType = 'snake-head';
      }
      // Check for food
      else if (food.position.x === x && food.position.y === y) {
        cellType = 'food';
      }

      cells.push(
        <div
          key={`${x}-${y}`}
          className={`cell ${cellType}`}
          style={{ width: '20px', height: '20px', border: '1px solid #ccc' }} // Basic styling
        ></div>
      );
    }
  }

  return (
    <div
      className="board"
      style={{
        display: 'grid',
        gridTemplateColumns: `repeat(${board.width}, 20px)`,
        gridTemplateRows: `repeat(${board.height}, 20px)`,
        width: `${board.width * 20 + board.width * 2}px`, // account for borders
        border: '1px solid black',
      }}
    >
      {cells}
    </div>
  );
};

export default BoardComponent;
