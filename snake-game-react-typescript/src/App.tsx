import React, { useState, useEffect, useCallback } from 'react';
import './App.css';
import BoardComponent from './components/BoardComponent';
import ScoreComponent from './components/ScoreComponent';
import GameControlsComponent from './components/GameControlsComponent';
import { GameBoard } from './game-logic/GameBoard';
import { Snake } from './game-logic/Snake';
import { Food } from './game-logic/Food';
import { Direction, Coordinates } from './game-logic/types';

const BOARD_WIDTH = 20;
const BOARD_HEIGHT = 20;
const INITIAL_SNAKE_POSITION: Coordinates = { x: 5, y: 5 };
const INITIAL_DIRECTION: Direction = "RIGHT";
const GAME_SPEED_MS = 200;

type GameState = 'initial' | 'playing' | 'gameOver' | 'paused';

function App() {
  const [gameBoard, setGameBoard] = useState<GameBoard | null>(null);
  const [snake, setSnake] = useState<Snake | null>(null);
  const [food, setFood] = useState<Food | null>(null);
  const [score, setScore] = useState<number>(0);
  const [gameState, setGameState] = useState<GameState>('initial');
  const [direction, setDirection] = useState<Direction>(INITIAL_DIRECTION);

  const initializeGame = useCallback(() => {
    const board = new GameBoard(BOARD_WIDTH, BOARD_HEIGHT);
    const initialSnake = new Snake(INITIAL_SNAKE_POSITION, INITIAL_DIRECTION);
    const initialFood = new Food(board.width, board.height, initialSnake.body);

    setGameBoard(board);
    setSnake(initialSnake);
    setFood(initialFood);
    setScore(0);
    setDirection(INITIAL_DIRECTION);
    setGameState('initial');
  }, []);

  useEffect(() => {
    initializeGame();
  }, [initializeGame]);

  const startGame = () => {
    if (gameState === 'initial' || gameState === 'gameOver') {
      initializeGame(); // Re-initialize for restart
    }
    setGameState('playing');
  };

  useEffect(() => {
    if (gameState !== 'playing' || !gameBoard) { // snake and food are accessed via their state updaters
      return;
    }

    const gameInterval = setInterval(() => {
      setSnake(prevSnake => {
        if (!prevSnake) return prevSnake;

        // Create a new snake instance for the current tick based on the previous state
        const currentTickSnake = new Snake(prevSnake.body[0], prevSnake.direction);
        currentTickSnake.body = [...prevSnake.body]; // Important: copy the body array

        currentTickSnake.changeDirection(direction); // Apply direction from keydown handler
        currentTickSnake.move();

        // Check for wall collision
        if (currentTickSnake.checkCollision(gameBoard.width, gameBoard.height)) {
          setGameState('gameOver');
          clearInterval(gameInterval); // Stop the loop
          return currentTickSnake; // Return the snake at point of collision
        }

        // Check for self-collision
        if (currentTickSnake.checkCollision(gameBoard.width, gameBoard.height, true)) {
          setGameState('gameOver');
          clearInterval(gameInterval); // Stop the loop
          return currentTickSnake; // Return the snake at point of collision
        }

        // Check for food consumption
        setFood(prevFood => {
          if (!prevFood) return prevFood;
          const head = currentTickSnake.body[0];
          if (head.x === prevFood.position.x && head.y === prevFood.position.y) {
            currentTickSnake.grow(); // Grow the current tick's snake
            setScore(prevScore => prevScore + 1);
            return new Food(gameBoard.width, gameBoard.height, currentTickSnake.body);
          }
          return prevFood; // No change to food
        });

        return currentTickSnake; // Set the new state for the snake
      });
    }, GAME_SPEED_MS);

    return () => clearInterval(gameInterval);
     // snake and food removed from deps: their updates are handled via functional setFood/setSnake
  }, [gameState, gameBoard, direction]);

  useEffect(() => {
    const handleKeyDown = (event: KeyboardEvent) => {
      let newDirection: Direction | null = null;
      switch (event.key) {
        case "ArrowUp":
          newDirection = "UP";
          break;
        case "ArrowDown":
          newDirection = "DOWN";
          break;
        case "ArrowLeft":
          newDirection = "LEFT";
          break;
        case "ArrowRight":
          newDirection = "RIGHT";
          break;
        default:
          return; // Ignore other keys
      }

      // The snake class's changeDirection method already prevents direct reversal.
      // We update the `direction` state, which the game loop then uses.
      // This ensures that the snake's own logic for changing direction is respected.
      // The `direction` state is updated, and the game loop will use this new direction
      // in the next tick, where `snake.changeDirection()` will validate it.
      if (newDirection) {
        setDirection(newDirection);
      }
    };

    window.addEventListener('keydown', handleKeyDown);
    return () => {
      window.removeEventListener('keydown', handleKeyDown);
    };
  }, []); // No dependencies needed, setDirection is stable and we're not using other state here.

  // Placeholder for App content, will be filled in next steps
  if (!gameBoard || !snake || !food) {
    return <div>Loading...</div>;
  }

  return (
    <div className="App">
      <header className="App-header">
        <h1>Snake Game</h1>
      </header>
      <main className="game-container">
        <ScoreComponent score={score} />
        <BoardComponent board={gameBoard} snake={snake} food={food} />
        <GameControlsComponent
          onStartGame={startGame}
          isGameOver={gameState === 'gameOver'}
        />
      </main>
    </div>
  );
}

export default App;
