using UnityEngine;
using UnityEngine.Tilemaps;

namespace Script.Level
{
    public class OverworldGenerator : LevelGenerator
    {
        [SerializeField] private Tile[] shadowTiles;
        [SerializeField] private Tilemap existingTilemapShadow;
        private Tilemap TilemapShadow;

        protected override void InitializeTilemap()
        {
            base.InitializeTilemap();
            TilemapShadow = DuplicateTilemap(existingTilemapShadow);
        }

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
            TilemapShadow.SetTile(new Vector3Int(targetPosition.x + 1, targetPosition.y), ShadowTileFor(code));
        }

        private Tile ShadowTileFor(int surroundingWallsCode)
        {
            return surroundingWallsCode switch
            {
                4 => shadowTiles[0], // Top
                12 => shadowTiles[0], // Top
                5 => shadowTiles[1], // Middle
                13 => shadowTiles[1], // Middle
                0 => shadowTiles[2], // Bottom
                1 => shadowTiles[2], // Bottom
                8 => shadowTiles[2], // Bottom
                9 => shadowTiles[2], // Bottom
                _ => null
            };
        }
    }
}