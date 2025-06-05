import React from 'react';

interface ScoreComponentProps {
  score: number;
}

const ScoreComponent: React.FC<ScoreComponentProps> = ({ score }) => {
  return (
    <div className="score">
      <h2>Score: {score}</h2>
    </div>
  );
};

export default ScoreComponent;
