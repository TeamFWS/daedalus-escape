using System.Collections.Generic;
using UnityEngine;

namespace Script.Level
{
    public class Maze
    {
        public const int NoneTile = 0;
        public const int PathTile = 1;
        public const int WallTile = 2;
        private static readonly Vector2Int[] Directions = { new(0, 2), new(0, -2), new(2, 0), new(-2, 0) };

        public Maze(int width, int height)
        {
            InitializeMaze(width, height);
            var startCell = new Vector2Int(1, 1);
            Tiles[startCell.x, startCell.y] = PathTile;
            var previousCells = new Stack<Vector2Int>();
            previousCells.Push(startCell);

            while (previousCells.Count > 0) GenerateNextPath(previousCells);
        }

        public int[,] Tiles { get; private set; }

        private void InitializeMaze(int width, int height)
        {
            Tiles = new int[width, height];
            for (var i = 0; i < width; i++)
            for (var j = 0; j < height; j++)
                Tiles[i, j] = WallTile;
        }

        private void GenerateNextPath(Stack<Vector2Int> previousCells)
        {
            var currentCell = previousCells.Peek();
            var availableDirections = GetShuffledDirections();

            var hasMoved = false;
            foreach (var direction in availableDirections)
            {
                var newCell = currentCell + direction;
                if (CanAddPath(newCell))
                {
                    AddPathToNewCell(currentCell, newCell);
                    previousCells.Push(newCell);
                    hasMoved = true;
                    break;
                }
            }

            if (!hasMoved) previousCells.Pop();
        }

        private static IEnumerable<Vector2Int> GetShuffledDirections()
        {
            var directions = (Vector2Int[])Directions.Clone();
            for (var i = 0; i < directions.Length; i++)
            {
                var randomIndex = Random.Range(i, directions.Length);
                (directions[i], directions[randomIndex]) = (directions[randomIndex], directions[i]);
            }

            return directions;
        }

        private bool CanAddPath(Vector2Int cell)
        {
            return cell.x >= 0 && cell.x < Tiles.GetLength(0) && cell.y >= 0 && cell.y < Tiles.GetLength(1) &&
                   Tiles[cell.x, cell.y] == WallTile;
        }

        private void AddPathToNewCell(Vector2Int currentCell, Vector2Int newCell)
        {
            Tiles[newCell.x, newCell.y] = PathTile;
            var midCell = currentCell + (newCell - currentCell) / 2;
            Tiles[midCell.x, midCell.y] = PathTile;
        }

        public void RemoveRandomWalls(float probability = 0.1f)
        {
            for (var i = 1; i < Tiles.GetLength(0) - 1; i++)
            for (var j = 1; j < Tiles.GetLength(1) - 1; j++)
                if (IsWallValidToRemove(i, j) && Random.Range(0f, 1f) < probability)
                    Tiles[i, j] = PathTile;
        }

        private bool IsWallValidToRemove(int i, int j)
        {
            return Tiles[i, j] == WallTile && GetSurroundingWallsCode(i, j) switch
            {
                1 => true,
                2 => true,
                4 => true,
                8 => true,
                5 => true,
                10 => true,
                _ => false
            };
        }

        public void Scale(int scaleValue)
        {
            var scaled = new int[Tiles.GetLength(0) * scaleValue, Tiles.GetLength(1) * scaleValue];

            for (var i = 0; i < scaled.GetLength(0); i++)
            for (var j = 0; j < scaled.GetLength(1); j++)
                scaled[i, j] = Tiles[i / scaleValue, j / scaleValue];

            Tiles = scaled;
        }

        public int GetSurroundingWallsCode(int i, int j)
        {
            var code = 0;
            if (j != Tiles.GetLength(1) - 1 && Tiles[i, j + 1] == WallTile) code += 1; // Top
            if (i != Tiles.GetLength(0) - 1 && Tiles[i + 1, j] == WallTile) code += 2; // Right
            if (j != 0 && Tiles[i, j - 1] == WallTile) code += 4; // Bottom
            if (i != 0 && Tiles[i - 1, j] == WallTile) code += 8; // Left
            return code;
        }
    }
}