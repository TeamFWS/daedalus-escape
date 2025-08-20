using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

namespace Script.Level
{
    public abstract class LevelGenerator : MonoBehaviour
    {
        private const string GeneratedSuffix = " - generated";
        [SerializeField] protected int mazeSize = 101;
        [SerializeField] protected Tile[] pathTiles;
        [SerializeField] protected Tile[] wallTiles;
        [SerializeField] protected Tilemap existingTilemapPath;
        [SerializeField] protected Tilemap existingTilemapWall;
        protected Tilemap TilemapPath;
        protected Tilemap TilemapWall;
        protected Tilemap TilemapWallTopEdge;

        public void Start()
        {
            DeleteGeneratedTilemap();
            InitializeTilemap();
            GenerateLevel();
            OnLevelGenerated?.Invoke();
        }

        public static event Action OnLevelGenerated;

        protected virtual void InitializeTilemap()
        {
            TilemapPath = DuplicateTilemap(existingTilemapPath);
            TilemapWall = DuplicateTilemap(existingTilemapWall);
            TilemapWallTopEdge = DuplicateTilemap(existingTilemapWall);
            TilemapWallTopEdge.GetComponent<TilemapCollider2D>().offset = Vector2.down * 0.5f;
            TilemapWallTopEdge.GetComponent<TilemapRenderer>().sortingOrder = 3;
        }

        private void GenerateLevel()
        {
            var maze = GenerateMaze();
            RemoveConflictingMazeTiles(maze, -mazeSize);
            AddMazeTiles(maze, -mazeSize);
        }

        protected abstract Maze GenerateMaze();

        private void RemoveConflictingMazeTiles(Maze maze, int shift)
        {
            for (var i = 0; i < maze.Tiles.GetLength(0); i++)
            for (var j = 0; j < maze.Tiles.GetLength(1); j++)
            {
                var targetPosition = new Vector3Int(shift + i, shift + j, 0);
                if (IsConflictingTile(targetPosition)) maze.Tiles[i, j] = Maze.NoneTile;
            }
        }

        private bool IsConflictingTile(Vector3Int position)
        {
            return existingTilemapPath.HasTile(position) || existingTilemapWall.HasTile(position);
        }

        private void AddMazeTiles(Maze maze, int shift)
        {
            for (var i = 0; i < maze.Tiles.GetLength(0); i++)
            for (var j = 0; j < maze.Tiles.GetLength(1); j++)
            {
                var targetPosition = new Vector3Int(shift + i, shift + j, 0);
                switch (maze.Tiles[i, j])
                {
                    case Maze.WallTile:
                        AddWallTile(maze.GetSurroundingWallsCode(i, j), targetPosition);
                        break;
                    case Maze.PathTile:
                        AddPathTile(targetPosition);
                        break;
                }
            }
        }

        protected virtual void AddWallTile(int code, Vector3Int targetPosition)
        {
            if (code is 4 or 6 or 12 or 14) // Is top edge
                TilemapWallTopEdge.SetTile(targetPosition, WallTileFor(code));
            else
                TilemapWall.SetTile(targetPosition, WallTileFor(code));
        }

        protected virtual void AddPathTile(Vector3Int targetPosition)
        {
            TilemapPath.SetTile(targetPosition, pathTiles[Random.Range(0, pathTiles.Length)]);
        }

        private Tile WallTileFor(int surroundingWallsCode)
        {
            return surroundingWallsCode switch
            {
                15 => wallTiles[0], // Middle
                6 => wallTiles[1], // Left-Top
                14 => wallTiles[2], // Top
                12 => wallTiles[3], // Right-Top
                3 => wallTiles[4], // Left-Bottom
                11 => wallTiles[5], // Bottom
                9 => wallTiles[6], // Right-Bottom
                7 => wallTiles[7], // Left
                13 => wallTiles[8], // Right
                10 => wallTiles[9], // Horizontal
                5 => wallTiles[10], // Vertical
                4 => wallTiles[11], // Single Top
                1 => wallTiles[12], // Single Bottom
                _ => wallTiles[13] // Single
            };
        }

        protected static Tilemap DuplicateTilemap(Tilemap tilemap)
        {
            var newTilemap = Instantiate(tilemap, tilemap.transform.parent, true);
            newTilemap.name = tilemap.name + GeneratedSuffix;
            newTilemap.ClearAllTiles();
            return newTilemap;
        }

        public void DeleteGeneratedTilemap()
        {
            if (!existingTilemapWall) return;
            var parentTransform = existingTilemapWall.transform.parent;

            for (var i = parentTransform.childCount - 1; i >= 0; i--)
            {
                var child = parentTransform.GetChild(i);
                if (child.name.EndsWith(GeneratedSuffix)) DestroyImmediate(child.gameObject);
            }
        }
    }
#if UNITY_EDITOR
    [CustomEditor(typeof(LevelGenerator), true)]
    public class LevelGeneratorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Generate"))
            {
                var generator = (LevelGenerator)target;
                generator.Start();
            }

            if (GUILayout.Button("Clear"))
            {
                var generator = (LevelGenerator)target;
                generator.DeleteGeneratedTilemap();
            }

            EditorGUILayout.EndHorizontal();
        }
    }
#endif
}