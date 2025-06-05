import React from 'react';
import { GameBoard } from '../game-logic/GameBoard';
import { Snake } from '../game-logic/Snake';
import { Food } from '../game-logic/Food';
// import { Coordinates } from '../game-logic/types'; // Coordinates not used in the new logic

interface BoardComponentProps {
  board: GameBoard;
  snake: Snake;
  food: Food;
}

const BoardComponent: React.FC<BoardComponentProps> = ({ board, snake, food }) => {
  const cells = [];

  for (let y = 0; y < board.height; y++) {
    for (let x = 0; x < board.width; x++) {
      // let cellType = 'empty'; // Not needed anymore
      // const currentPos: Coordinates = { x, y }; // Not needed anymore

      let cellContent = null;
      const isHead = snake.body[0].x === x && snake.body[0].y === y;
      // Ensure snake.body exists and has elements before accessing slice
      const isBody = snake.body && snake.body.length > 1 && snake.body.slice(1).some(segment => segment.x === x && segment.y === y);
      const isFood = food.position.x === x && food.position.y === y;

      if (isHead) {
        cellContent = <div style={{ width: '100%', height: '100%', backgroundColor: 'green' }}></div>;
      } else if (isBody) {
        cellContent = <div style={{ width: '100%', height: '100%', backgroundColor: 'lightgreen' }}></div>;
      } else if (isFood) {
        cellContent = <div style={{ width: '100%', height: '100%', backgroundColor: 'red' }}></div>;
      }

      cells.push(
        <div
          key={`${x}-${y}`}
          className="cell" // Keep basic cell class for grid layout
          style={{ width: '20px', height: '20px', border: '1px solid #ccc', position: 'relative' }} // Added position relative for img positioning
        >
          {cellContent}
        </div>
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
