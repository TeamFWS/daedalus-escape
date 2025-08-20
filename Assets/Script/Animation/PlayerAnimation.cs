using DG.Tweening;
using UnityEngine;

namespace Script.Animation
{
    public class PlayerAnimation
    {
        private readonly SpriteRenderer _spriteRenderer;
        private readonly Transform _transform;

        public PlayerAnimation(SpriteRenderer spriteRenderer, Transform transform)
        {
            _spriteRenderer = spriteRenderer;
            _transform = transform;
        }

        public Tween AnimateEntering(Transform entrance)
        {
            return DOTween.Sequence()
                .Join(_spriteRenderer.DOColor(new Color(0f, 0f, 0f, 0f), 0.9f))
                .Join(_transform.DOScale(0.9f, 1.0f))
                .Join(_transform.DOMove(entrance.position, 1.0f));
        }

        public void MovePlayerByEntrance(Transform entrance)
        {
            _transform.DOMove(entrance.position + Vector3.down, 0);
            _transform.DOScale(1f, 0);
            _spriteRenderer.DOColor(Color.white, 0);
        }
    }
}