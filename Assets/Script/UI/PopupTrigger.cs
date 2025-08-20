using DG.Tweening;
using Script.Animation;
using Unity.VisualScripting;
using UnityEngine;

namespace Script.UI
{
    public class PopupTrigger : MonoBehaviour
    {
        [SerializeField] private GameObject popup;
        [SerializeField] private LevelEnd levelEnd;

        private void Start()
        {
            popup.SetActive(false);
        }

        public void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player") && !levelEnd.AreKeysCollected())
            {
                ScaleAnimation.ScaleY(popup);
                popup.GameObject().SetActive(true);
            }
        }

        public void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
                ScaleAnimation.ScaleY(popup, reverse: true).OnComplete(() => { popup.GameObject().SetActive(false); });
        }
    }
}