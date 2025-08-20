using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace Script.Puzzle
{
    public class PuzzleScene
    {
        private readonly string _name;
        private readonly int _randomSeed = DateTime.Now.Millisecond;
        private int _activatedPuzzleIndex;

        private Camera _camera;
        private Transform _player;
        private Transform[] _startPoints;

        public PuzzleScene(string name)
        {
            _name = name;
        }

        public void LoadPuzzle(int index)
        {
            _activatedPuzzleIndex = index;
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.LoadScene(_name, LoadSceneMode.Additive);
        }

        public void Reload()
        {
            SceneManager.sceneUnloaded += OnSceneUnloaded;
            Close();
        }

        private void OnSceneUnloaded(Scene scene)
        {
            if (scene.name != _name) return;
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
            LoadPuzzle(_activatedPuzzleIndex);
        }

        public void Close()
        {
            SceneManager.UnloadSceneAsync(_name);
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name != _name) return;
            SetupPuzzleScene();
            MoveToActivatedPuzzle();
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void SetupPuzzleScene()
        {
            _player = GameObject.FindWithTag("Player").GetComponent<Transform>();
            _camera = GameObject.FindWithTag("PuzzleCamera").GetComponent<Camera>();
            SetupStartPoints(GameObject.FindWithTag("PuzzleStartPoints").GetComponent<Transform>());
        }

        private void SetupStartPoints(Transform startPointsContainer)
        {
            var childCount = startPointsContainer.childCount;
            _startPoints = new Transform[childCount];

            for (var i = 0; i < childCount; i++) _startPoints[i] = startPointsContainer.GetChild(i);
            ShuffleStartPoints();
        }

        private void ShuffleStartPoints()
        {
            Random.InitState(_randomSeed);
            for (var i = 0; i < _startPoints.Length; i++)
            {
                var randomIndex = Random.Range(i, _startPoints.Length);
                (_startPoints[i], _startPoints[randomIndex]) = (_startPoints[randomIndex], _startPoints[i]);
            }
        }

        private void MoveToActivatedPuzzle()
        {
            var startPoint = _startPoints[_activatedPuzzleIndex];
            var cameraSize = startPoint.localScale.x;

            _player.transform.position = startPoint.position;
            _camera.transform.position = startPoint.position;
            _camera.orthographicSize = cameraSize;
        }
    }
}