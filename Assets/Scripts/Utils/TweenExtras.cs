using DG.Tweening;
using UnityEngine;

namespace Utils
{
    public static class TweenExtras
    {
        public static Sequence DoJumpRandomInCircle(this Transform transform, float power = 1f, float time = 0.1f)
        {
            var rndCircle = Random.insideUnitSphere + transform.position;
            rndCircle.y = transform.position.y;
            return transform.DOJump(rndCircle, power, 1, time);
        }
    }
}