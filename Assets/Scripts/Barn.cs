using System.Collections;
using DG.Tweening;
using Player;
using UI;
using UnityEngine;

public class Barn : MonoBehaviour
{
    [SerializeField, Tooltip("Plants will be move to this point from player")] 
    private Transform sellPoint;
    
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (!other.TryGetComponent(out PlayerBackpack backpack)) return;
        if (backpack.HasItems)
        {
            StartCoroutine(nameof(SellItems), backpack);
        }
    }

    private IEnumerator SellItems(PlayerBackpack backpack)
    {
        var delay = new WaitForSeconds(0.01f);
        while (backpack.HasItems)
        {
            var item = backpack.PopItem();
            var rndInCircle = Random.insideUnitCircle;
            var toSellPos = sellPoint.position + new Vector3(rndInCircle.x, 0, rndInCircle.y);
            DOTween.Sequence()
                .Append(item.transform.DOMove(toSellPos, 0.3f))
                .AppendCallback(() =>
                {
                    CoinsDisplayUI.Instance.CollectCoin(sellPoint.position, 15);
                    Destroy(item.gameObject); // TODO: pool objects
                });
            yield return delay;
        }
    }
}