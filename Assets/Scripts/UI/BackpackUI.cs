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
        [Space]
        [SerializeField] private TextMeshProUGUI inBackpackAmount;
        [SerializeField] private Image fillBar;
        [Space]
        [SerializeField] private TextMeshProUGUI fullText;

        private void Start()
        {
            UpdateInfo();
            playerBackpack.OnCollectedChange += UpdateInfo;
            playerBackpack.OnBackpackFull += ShowBackpackFull;
            fullText.rectTransform.localScale = Vector3.zero;
        }

        private void ShowBackpackFull()
        {
            DOTween.Sequence()
                .Append(fullText.rectTransform.DOScale(1f, .5f))
                .AppendInterval(2f)
                .Append(fullText.rectTransform.DOScale(0f, .5f));
        }

        private void UpdateInfo()
        {
            inBackpackAmount.text = $"{playerBackpack.CurrentCollectedCount}/{playerBackpack.StackSize}";
            fillBar.DOFillAmount((float)playerBackpack.CurrentCollectedCount / playerBackpack.StackSize, 0.1f);
        }
    }
}
