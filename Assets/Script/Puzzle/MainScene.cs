using UnityEngine;

namespace Script.Puzzle
{
    public class MainScene
    {
        private readonly Camera _camera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        private readonly GameObject _player = GameObject.FindWithTag("Player");
        private readonly GameObject _scene = GameObject.FindWithTag("Scene");

        public void SetActive(bool value)
        {
            _scene.SetActive(value);
            _player.SetActive(value);
            _camera.enabled = value;
        }
    }
}