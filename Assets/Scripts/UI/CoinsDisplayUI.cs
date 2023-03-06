using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace UI
{
    public class CoinsDisplayUI : MonoBehaviour
    {
        public static CoinsDisplayUI Instance { get; private set; }
        
        [SerializeField] private Image coinPrefab;
        [SerializeField] private TextMeshProUGUI coinsAmountText;
        private readonly List<Image> coinsPool = new List<Image>();
        private Camera mainCam;
        
        // TODO: move coins store to another file
        private int coins; 
        private int inAnimationCoins; // coins with playing animation
        private int animationEndedCoins; // coin move animation ended, adding now

        private Tweener amountTweener;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
#if DEBUG
            else Debug.LogError($"Should be only 1 CoinsDisplayUI! {nameof(CoinsDisplayUI)} copy of object exist! ");
#endif
        }

        private void Start()
        {
            amountTweener = coinsAmountText.rectTransform
                .DOShakePosition(0.2f, 5f)
                .SetAutoKill(false);
            mainCam = Camera.main;
            for (var i = 0; i < 5; i++)
            {
                AddCoinToPool();
            }
        }

        private void OnDestroy()
        {
            coins += inAnimationCoins;
            coins += animationEndedCoins;
        }

        private Image AddCoinToPool()
        {
            var coin = Instantiate(coinPrefab, transform);
            coin.rectTransform.localScale = Vector3.zero;;
            coin.gameObject.SetActive(false);
            coinsPool.Add(coin);
            return coin;
        }

        private Image GetFreeCoin()
        {
            var i = coinsPool.FirstOrDefault(i => !i.gameObject.activeSelf);
            return i ? i : AddCoinToPool();
        }

        public void CollectCoin(Vector3 worldPos, int coinsAmount)
        {
            var freeCoin = GetFreeCoin();
            var rndInCircle = Random.insideUnitCircle;
            var canvasPos = mainCam.WorldToScreenPoint(worldPos + new Vector3(rndInCircle.x, 0, rndInCircle.y));
            canvasPos.z = 0;
            freeCoin.rectTransform.position = canvasPos;
            freeCoin.gameObject.SetActive(true);
            inAnimationCoins += coinsAmount;
            var sequence = DOTween.Sequence();
            sequence
                .SetEase(Ease.Flash)
                .Append(freeCoin.rectTransform.DOScale(.7f, 0.1f))
                .SetEase(Ease.InExpo)
                .Append(freeCoin.rectTransform.DOAnchorPos(coinsAmountText.rectTransform.localPosition, 0.5f))
                .SetEase(Ease.Flash)
                .Append(freeCoin.rectTransform.DOScale(0f, 0.1f))
                .AppendCallback(() =>
                {
                    freeCoin.gameObject.SetActive(false);
                    animationEndedCoins += coinsAmount;
                    inAnimationCoins -= coinsAmount;
                    amountTweener.Restart();
                });
        }
        
        private void FixedUpdate()
        {
            if (animationEndedCoins <= 0) return;
            for (var bound = 1000; bound > 10; bound /= 10)
            {
                if (animationEndedCoins < bound) continue;
                coins += bound;
                animationEndedCoins -= bound;
                coinsAmountText.text = coins.ToString();
                return;
            }
            coins += 1;
            animationEndedCoins -= 1;
            coinsAmountText.text = coins.ToString();
        }
        
#if DEBUG // TODO: DELETE
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                CollectCoin(new Vector3(-2.5f, 0, -2.5f), 15); // TODO: DELETE
            }
        }
#endif
    }
}
