using System;
using System.Collections.Generic;
using System.Linq;

namespace SnakeGameCSharp
{
    public class GameBoard
    {
        private readonly int _width;
        private readonly int _height;
        private List<List<char>> _board;

        public GameBoard(int width, int height)
        {
            _width = width;
            _height = height;
            InitializeBoard();
        }

        private void InitializeBoard()
        {
            _board = new List<List<char>>();
            for (int i = 0; i < _height; i++)
            {
                _board.Add(new List<char>(Enumerable.Repeat(' ', _width).ToList()));
            }
        }

        public void Display()
        {
            foreach (var row in _board)
            {
                Console.WriteLine(new string(row.ToArray()));
            }
        }

        public void Clear()
        {
            InitializeBoard(); // Re-initialize to clear
        }

        public void DrawElement(int x, int y, char character)
        {
            if (y >= 0 && y < _height && x >= 0 && x < _width)
            {
                _board[y][x] = character;
            }
        }
    }
}
