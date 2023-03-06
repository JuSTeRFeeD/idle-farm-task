using System;
using DG.Tweening;
using Player;
using Scriptable;
using UnityEngine;
using Utils;
using Random = UnityEngine.Random;

public class Plant : MonoBehaviour
{
    [SerializeField] private MeshRenderer stackMeshRenderer;
    [SerializeField] private ParticleSystem cutOffParticle;
    [SerializeField] private Collider triggerCollider;

    private GameObject plantGameObject;
    
    private Vector2Int plantPos;
    private bool isCollected;
    private bool canPickup;
    private bool isCuttedOff;
    
    public event Action<Vector2Int> OnPickup;
    public PlantData PlantData { get; private set; }

    private void Awake()
    {
        stackMeshRenderer.gameObject.SetActive(false);
    }

    public void Setup(Vector2Int cellPos, PlantData plantData)
    {
        plantGameObject = Instantiate(plantData.plantPrefab, transform.position, Quaternion.identity, transform);
        PlantData = plantData;
        plantPos = cellPos;
    }

    public void Pickup(PlayerBackpack backpack)
    {
        if (!canPickup || isCollected) return;
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
        DOTween.Sequence()
            .Append(transform.DoJumpRandomInCircle(1, 0.5f))
            .AppendCallback(() =>
            {
                stackMeshRenderer.sharedMaterial = PlantData.stackMaterial;
                stackMeshRenderer.gameObject.SetActive(true);
                transform.localRotation = Quaternion.Euler(0,Random.Range(0, 360f), 0);
                plantGameObject.SetActive(false);
                canPickup = true;
            });
    }
}
