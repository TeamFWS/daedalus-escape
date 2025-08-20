using DG.Tweening;
using UnityEngine;

namespace Script.Animation
{
    public static class ScaleAnimation
    {
        public static Tween ScaleX(GameObject toAnimate, float duration = 0.5f, bool reverse = false)
        {
            toAnimate.transform.localScale = new Vector3(reverse ? 1f : 0f, 1f, 1f);
            return DOTween.Sequence().Join(toAnimate.transform.DOScaleX(reverse ? 0f : 1f, duration));
        }

        public static Tween ScaleY(GameObject toAnimate, float duration = 0.5f, bool reverse = false)
        {
            toAnimate.transform.localScale = new Vector3(1f, reverse ? 1f : 0f, 1f);
            return DOTween.Sequence().Join(toAnimate.transform.DOScaleY(reverse ? 0f : 1f, duration));
        }
    }
}