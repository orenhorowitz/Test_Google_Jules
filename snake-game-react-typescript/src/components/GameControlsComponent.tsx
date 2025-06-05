import React from 'react';

interface GameControlsComponentProps {
  onStartGame: () => void;
  onPauseGame?: () => void; // Optional
  isGameOver: boolean;
  isGamePaused?: boolean; // Optional
}

const GameControlsComponent: React.FC<GameControlsComponentProps> = ({
  onStartGame,
  onPauseGame,
  isGameOver,
  isGamePaused,
}) => {
  return (
    <div className="game-controls">
      {isGameOver && (
        <div>
          <h2>Game Over!</h2>
          <button onClick={onStartGame}>Restart Game</button>
        </div>
      )}
      {!isGameOver && (
        <>
          <button onClick={onStartGame} disabled={!isGameOver && isGamePaused === false}>
            Start Game
          </button>
          {onPauseGame && (
            <button onClick={onPauseGame} disabled={isGamePaused === undefined}>
              {isGamePaused ? 'Resume' : 'Pause'}
            </button>
          )}
        </>
      )}
    </div>
  );
};

export default GameControlsComponent;
