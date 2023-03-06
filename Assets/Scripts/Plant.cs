using System;
using Player;
using Scriptable;
using UnityEngine;
using Utils;

public class Plant : MonoBehaviour
{
    [SerializeField] private ParticleSystem cutOffParticle;
    [SerializeField] private Collider triggerCollider;

    private Vector2Int plantPos;
    private bool isCollected;
    private bool isCuttedOff;
    
    public event Action<Vector2Int> OnPickup;
    public PlantData PlantData { get; private set; }
    

    public void Setup(Vector2Int cellPos, PlantData plantData)
    {
        Instantiate(plantData.stackPrefab, transform.position, Quaternion.identity, transform);
        PlantData = plantData;
        plantPos = cellPos;
    }

    public void Pickup(PlayerBackpack backpack)
    {
        if (!isCuttedOff || isCollected) return;
        if (!backpack.AddItem(this)) return;
        isCollected = true;
        OnPickup?.Invoke(plantPos);
        OnPickup = null;
        triggerCollider.enabled = false;
    }

    public void CutOff()
    {
        if (isCuttedOff) return;
        isCuttedOff = true;
        cutOffParticle.Play();
        transform.DoJumpRandomInCircle();
    }
}
