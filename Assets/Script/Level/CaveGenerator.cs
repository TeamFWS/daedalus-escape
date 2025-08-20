using UnityEngine;

namespace Script.Level
{
    public class CaveGenerator : LevelGenerator
    {
        private const float DecorativePathProbability = 0.1f;

        protected override Maze GenerateMaze()
        {
            var maze = new Maze(mazeSize, mazeSize);
            maze.RemoveRandomWalls();
            maze.Scale(2);
            return maze;
        }

        protected override void AddWallTile(int code, Vector3Int targetPosition)
        {
            base.AddWallTile(code, targetPosition);
            TilemapPath.SetTile(targetPosition, pathTiles[0]); // Fill up transparent part of cave tile
        }

        protected override void AddPathTile(Vector3Int targetPosition)
        {
            var pathTile = Random.Range(0f, 1f) > DecorativePathProbability
                ? pathTiles[Random.Range(0, 4)] // First 4 tiles are simple ground
                : pathTiles[Random.Range(4, pathTiles.Length)];

            TilemapPath.SetTile(targetPosition, pathTile);
        }
    }
}