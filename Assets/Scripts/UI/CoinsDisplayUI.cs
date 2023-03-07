using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using Random = UnityEngine.Random;

namespace UI
{
    public class CoinsDisplayUI : MonoBehaviour
    {
        public static CoinsDisplayUI Instance { get; private set; }
        private PlayerInfo playerInfo;
        
        [SerializeField] private Image coinPrefab;
        [SerializeField] private int poolCount = 40;
        [Space]
        [SerializeField] private TextMeshProUGUI coinsAmountText;
        
        private readonly List<Image> coinsPool = new List<Image>();
        private Camera mainCam;
        
        private int inAnimationCoins; // coins playing move animation
        private int animationEndedCoins; // coins move animation ended (adding now in fixedUpdate)

        private Tweener amountTweener;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
#if DEBUG
                Debug.LogError($"Should be only 1 CoinsDisplayUI instance! {nameof(CoinsDisplayUI)} copy!");
#endif
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            playerInfo = PlayerInfo.Instance;
            playerInfo.OnCoinsChange += HandleCoinsChange;
            
            amountTweener = coinsAmountText.rectTransform
                .DOShakePosition(0.2f, 5f)
                .SetAutoKill(false);
            mainCam = Camera.main;
            for (var i = 0; i < poolCount; i++)
            {
                AddCoinToPool();
            }
        }

        private void OnDestroy()
        {
            playerInfo.OnCoinsChange -= HandleCoinsChange;
            playerInfo.AddCoins(inAnimationCoins);
            playerInfo.AddCoins(animationEndedCoins);
        }

        private void HandleCoinsChange() => coinsAmountText.text = StringExtras.FormatAmount(playerInfo.Coins);

        private Image AddCoinToPool()
        {
            var coin = Instantiate(coinPrefab, transform);
            coin.rectTransform.localScale = Vector3.one * .5f;
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
            var rndInCircle = Random.insideUnitCircle;
            var canvasPos = mainCam.WorldToScreenPoint(worldPos + new Vector3(rndInCircle.x, 0, rndInCircle.y));
            canvasPos.z = 0;
            
            var freeCoin = GetFreeCoin();
            freeCoin.rectTransform.position = canvasPos;
            freeCoin.gameObject.SetActive(true);
            
            inAnimationCoins += coinsAmount;
            var sequence = DOTween.Sequence();
            sequence
                .SetEase(Ease.InExpo)
                .Append(freeCoin.rectTransform.DOAnchorPos(coinsAmountText.rectTransform.localPosition, 0.5f))
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
            
            // for adding bound instantly w/o waiting
            if (animationEndedCoins > StringExtras.FormatBound || playerInfo.Coins > StringExtras.FormatBound)
            {
                for (var bound = 10000000; bound > StringExtras.FormatBound; bound /= 1000)
                {
                    if (animationEndedCoins < bound) continue;
                    playerInfo.AddCoins(bound);
                    animationEndedCoins -= bound;
                    return;
                }

                playerInfo.AddCoins(animationEndedCoins);
                animationEndedCoins = 0;
            }
            else
            {
                for (var bound = StringExtras.FormatBound; bound > 10; bound /= 10)
                {
                    if (animationEndedCoins < bound) continue;
                    playerInfo.AddCoins(bound);
                    animationEndedCoins -= bound;
                    return;
                }

                playerInfo.AddCoins(1);
                animationEndedCoins -= 1;
            }
        }
        
#if DEBUG
        private void Update() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha1)) CollectCoin(Vector3.zero, 123);
            if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha2)) CollectCoin(Vector3.zero, 1235);
            if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha3)) CollectCoin(Vector3.zero, 12356);
            if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha4)) CollectCoin(Vector3.zero, 123567);
        }
#endif
    }
}
