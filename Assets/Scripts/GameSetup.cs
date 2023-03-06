using DG.Tweening;
using UnityEngine;

public class GameSetup : MonoBehaviour
{
    private void Awake()
    {
        Application.targetFrameRate = 60;
        DOTween.SetTweensCapacity(200, 125);
    }
}
