using Player;
using UnityEngine;
using UnityEngine.Events;

public class Plant : MonoBehaviour
{
    [SerializeField] private Collider triggerCollider;

    private int plantIndex;
    private bool isCollected;
    private bool isCuttedOff;
    
    public UnityAction<int> OnPickup;

    public void Setup(int index)
    {
        plantIndex = index;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isCuttedOff || isCollected || !other.CompareTag("Player")) return;
        if (!other.TryGetComponent(out PlayerBackpack backpack) || !backpack.AddItem(this)) return;
        isCollected = true;
        OnPickup?.Invoke(plantIndex);
        OnPickup = null;
        triggerCollider.enabled = false;
    }

    public void CutOff()
    {
        if (isCuttedOff) return;
        isCuttedOff = true;
    }
}
