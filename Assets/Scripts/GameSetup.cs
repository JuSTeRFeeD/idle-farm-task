using DG.Tweening;
using UnityEngine;

public class GameSetup : MonoBehaviour
{
    private void Awake()
    {
#if !DEBUG
        Application.targetFrameRate = 60;
#endif
        DOTween.SetTweensCapacity(200, 125);
    }
}
