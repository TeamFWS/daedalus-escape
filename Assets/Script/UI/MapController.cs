using System.Collections.Generic;
using Script.Level;
using Script.Puzzle;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Script.UI
{
    public class MapController : MonoBehaviour
    {
        private const float Margin = 0.1f;
        [SerializeField] private RectTransform mapWindow;
        [SerializeField] private RectTransform mapBackground;
        [SerializeField] private RectTransform playerIcon;
        [SerializeField] private GameObject entranceIcon;
        [SerializeField] private GameObject exitIcon;
        private readonly List<GameObject> _spawnedEntranceIcons = new();
        private Transform _player;
        private Bounds _sceneBounds;

        private void Start()
        {
            _player = GameObject.FindWithTag("Player").GetComponent<Transform>();
            mapWindow.GameObject().SetActive(false);
        }

        private void Update()
        {
            if (isActiveAndEnabled)
                ShowOnMap(playerIcon, _player.transform.position);
        }

        private void OnEnable()
        {
            PuzzleManager.OnPuzzleFinished += ResetPuzzleEntrances;
            LevelGenerator.OnLevelGenerated += Initialize;
        }

        private void OnDisable()
        {
            PuzzleManager.OnPuzzleFinished -= ResetPuzzleEntrances;
            LevelGenerator.OnLevelGenerated -= Initialize;
        }

        private void Initialize()
        {
            CalculateSceneBounds();
            UpdateMapBackground();
            ResetPuzzleEntrances();
            ShowMapExit();
        }

        private void CalculateSceneBounds()
        {
            var tilemaps = GameObject.FindWithTag("Scene").GetComponentsInChildren<Tilemap>();
            _sceneBounds = tilemaps[0].localBounds;
            foreach (var tilemap in tilemaps) _sceneBounds.Encapsulate(tilemap.localBounds);
        }

        private void UpdateMapBackground()
        {
            var availableWidth = mapWindow.rect.width * (1 - 2 * Margin);
            var availableHeight = mapWindow.rect.height * (1 - 2 * Margin);
            var availableAspectRatio = availableWidth / availableHeight;

            var sceneBoundsAspectRatio = _sceneBounds.size.x / _sceneBounds.size.y;

            float rectWidth, rectHeight;

            if (sceneBoundsAspectRatio > availableAspectRatio)
            {
                rectWidth = availableWidth;
                rectHeight = rectWidth / sceneBoundsAspectRatio;
            }
            else
            {
                rectHeight = availableHeight;
                rectWidth = rectHeight * sceneBoundsAspectRatio;
            }

            mapBackground.sizeDelta = new Vector2(rectWidth, rectHeight);
            mapBackground.anchoredPosition = new Vector2(
                mapWindow.rect.width * Margin + (availableWidth - rectWidth) / 2f,
                mapWindow.rect.height * Margin + (availableHeight - rectHeight) / 2f);
        }

        private void ResetPuzzleEntrances()
        {
            foreach (var icon in _spawnedEntranceIcons) Destroy(icon);
            _spawnedEntranceIcons.Clear();

            var puzzleEntrances = GameObject.FindGameObjectsWithTag("PuzzleEntrance");
            foreach (var entrance in puzzleEntrances)
            {
                var icon = Instantiate(entranceIcon, mapBackground).GetComponent<RectTransform>();
                ShowOnMap(icon, entrance.transform.position);
                _spawnedEntranceIcons.Add(icon.gameObject);
            }
        }

        private void ShowMapExit()
        {
            ShowOnMap(Instantiate(exitIcon, mapBackground).GetComponent<RectTransform>(),
                GameObject.FindGameObjectWithTag("MapExit").transform.position);
        }

        private void ShowOnMap(RectTransform icon, Vector3 position)
        {
            var normalizedX = Mathf.Clamp01((position.x - _sceneBounds.min.x) / _sceneBounds.size.x);
            var normalizedY = Mathf.Clamp01((position.y - _sceneBounds.min.y) / _sceneBounds.size.y);

            icon.anchoredPosition = new Vector2(
                normalizedX * mapBackground.rect.width,
                normalizedY * mapBackground.rect.height);
        }
    }
}