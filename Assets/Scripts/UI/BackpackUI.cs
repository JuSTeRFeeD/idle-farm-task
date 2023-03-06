using DG.Tweening;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class BackpackUI : MonoBehaviour
    {
        [SerializeField] private PlayerBackpack playerBackpack;
        [SerializeField] private TextMeshProUGUI inBackpackAmount;
        [SerializeField] private Image fillBar;

        private void Start()
        {
            UpdateInfo();
            playerBackpack.OnCollectedChange += UpdateInfo;
        }

        private void UpdateInfo()
        {
            inBackpackAmount.text = $"{playerBackpack.CurrentCollectedCount}/{playerBackpack.StackSize}";
            fillBar.DOFillAmount((float)playerBackpack.CurrentCollectedCount / playerBackpack.StackSize, 0.1f);
        }
    }
}
