using System;
using System.Collections.Generic;
using UnityEngine;

namespace Script.Puzzle
{
    public class PuzzleManager : MonoBehaviour
    {
        [SerializeField] private string puzzleSceneName;
        [SerializeField] private GameObject puzzleUI;
        [SerializeField] private GameObject levelUI;
        private readonly List<int> _entranceIDs = new();
        private bool _isPuzzleSceneActivated;
        private GameObject _lastEntrance;
        private MainScene _mainScene;
        private PuzzleScene _puzzleScene;

        private void Start()
        {
            if (string.IsNullOrEmpty(puzzleSceneName)) throw new ArgumentException("Puzzle scene name is not set!");
            if (puzzleUI == null) throw new ArgumentException("PuzzleUI is not assigned!");
            if (levelUI == null) throw new ArgumentException("LevelUI is not assigned!");

            _mainScene = new MainScene();
            _puzzleScene = new PuzzleScene(puzzleSceneName);
            ToggleVisibility();
            ResetKeys();
        }

        public static event Action OnPuzzleFinished;

        public void OpenPuzzleForID(GameObject entrance)
        {
            _lastEntrance = entrance;
            var entranceID = entrance.GetInstanceID();

            if (!_entranceIDs.Contains(entranceID))
                _entranceIDs.Add(entranceID);

            LoadPuzzleScene(entranceID);
        }

        private void LoadPuzzleScene(int entranceID)
        {
            if (_isPuzzleSceneActivated) return;
            _isPuzzleSceneActivated = true;
            _puzzleScene.LoadPuzzle(_entranceIDs.IndexOf(entranceID));
            ToggleVisibility();
        }

        public void ClosePuzzleScene()
        {
            _puzzleScene.Close();
            _isPuzzleSceneActivated = false;
            ToggleVisibility();
        }

        private void ToggleVisibility()
        {
            _mainScene.SetActive(!_isPuzzleSceneActivated);
            levelUI.SetActive(!_isPuzzleSceneActivated);
            puzzleUI.SetActive(_isPuzzleSceneActivated);
        }

        public void ReloadPuzzleScene()
        {
            _puzzleScene.Reload();
        }

        public void CurrentPuzzleFinished()
        {
            _lastEntrance.GetComponent<BoxCollider2D>().isTrigger = false;
            _lastEntrance.tag = "Untagged";
            ClosePuzzleScene();
            OnPuzzleFinished?.Invoke();
        }

        private void ResetKeys()
        {
            for (var i = 1; i <= 4; i++)
            for (var j = 1; j <= 3; j++)
                PlayerPrefs.SetInt("keylvl" + i + "number" + j, 0);
        }
    }
}