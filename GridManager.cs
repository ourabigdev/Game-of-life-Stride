using System;
using System.Linq;

namespace Game_of_life
{
    public class GridManager
    {
        private bool[,] _currentGrid;
        private bool[,] _nextGrid;

        public GridManager(int width, int height)
        {
            _currentGrid = new bool[width, height];
            _nextGrid = new bool[width, height];
        }

        public bool[,] CurrentGrid => _currentGrid;
        public bool[,] NextGrid => _nextGrid;

        public void InitializeGrid()
        {
            Array.Clear(_currentGrid, 0, _currentGrid.Length);
        }

        public void ToggleCell(int x, int y, bool toggle)
        {
            if (IsWithinBounds(x, y))
            {
                _currentGrid[x, y] = toggle;
            }
        }

        public void UpdateGrid()
        {
            for (int x = 0; x < GameSettings.GridWidth; x++)
            {
                for (int y = 0; y < GameSettings.GridHeight; y++)
                {
                    int aliveNeighbors = GetAliveNeighbors(x, y);

                    bool shouldLive = GameSettings.SurvivalRules.Contains(aliveNeighbors) && _currentGrid[x, y];
                    bool shouldBeBorn = GameSettings.BirthRules.Contains(aliveNeighbors);

                    _nextGrid[x, y] = shouldBeBorn || shouldLive;
                }
            }

            var temp = _currentGrid;
            _currentGrid = _nextGrid;
            _nextGrid = temp;
        }

        private int GetAliveNeighbors(int x, int y)
        {
            int count = 0;

            for (int dx = -1; dx <= 1; dx++)
            {
                for (int dy = -1; dy <= 1; dy++)
                {
                    if (dx == 0 && dy == 0) continue;

                    int nx = x + dx;
                    int ny = y + dy;

                    if (IsWithinBounds(nx, ny) && _currentGrid[nx, ny])
                        count++;
                }
            }

            return count;
        }

        private bool IsWithinBounds(int x, int y)
        {
            return x >= 0 && x < GameSettings.GridWidth && y >= 0 && y < GameSettings.GridHeight;
        }
    }
}
