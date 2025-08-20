using DG.Tweening;
using Script.Animation;
using UnityEngine;

namespace Script.Puzzle
{
    public class PuzzleTrigger : MonoBehaviour
    {
        private CutoffMask _cutoffMask;
        private PlayerAnimation _playerAnimation;
        private PuzzleManager _puzzleManager;

        private void Start()
        {
            _puzzleManager = FindObjectOfType<PuzzleManager>();
            _cutoffMask = FindObjectOfType<CutoffMask>();
            _playerAnimation = new PlayerAnimation(GetComponent<SpriteRenderer>(), transform);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("PuzzleEntrance")) HandlePuzzleEntrance(collision.gameObject);
        }

        private void HandlePuzzleEntrance(GameObject puzzleEntrance)
        {
            _playerAnimation.AnimateEntering(puzzleEntrance.transform).OnComplete(() =>
            {
                _cutoffMask.StartTransition();
                _playerAnimation.MovePlayerByEntrance(puzzleEntrance.transform);
                _puzzleManager.OpenPuzzleForID(puzzleEntrance);
            });
        }
    }
}