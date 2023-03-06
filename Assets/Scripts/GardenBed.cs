using Scriptable;
using UnityEngine;

internal class PlantCell
{
    public bool IsPickedUp;
    public float NextSpawnTime;
    public Vector2Int CellPos;
}

public class GardenBed : MonoBehaviour
{
    [SerializeField] private PlantData plantData;
    [SerializeField] private Plant plantPrefab;
    [SerializeField] private float respawnTime = 10f;
    [Space]
    [SerializeField, Min(2)] private int sizeX = 10;
    [SerializeField, Min(2)] private int sizeY = 7;

    private PlantCell[,] plants;

    private readonly Vector3 halfCellSize = new Vector3(0.5f, 0, 0.5f);
    
    private void Start()
    {
        plants = new PlantCell[sizeX, sizeY];
        for (var x = 0; x < sizeX; x++)
        {
            for (var y = 0; y < sizeY; y++)
            {
                SpawnPlant(x, y);
            }
        }
    }

    private void SpawnPlant(int x, int y)
    {
        var plant = Instantiate(plantPrefab, GetPositionByCellPos(x, y), Quaternion.identity, transform);
        plants[x, y] = new PlantCell
        {
            CellPos = new Vector2Int(x, y),
            IsPickedUp = false,
            NextSpawnTime = 0
        };
        plant.Setup(new Vector2Int(x, y), plantData);
        plant.OnPickup += HandlePickupPlant;
    }

    private void HandlePickupPlant(Vector2Int cellPos)
    {
        plants[cellPos.x, cellPos.y].IsPickedUp = true;
        plants[cellPos.x, cellPos.y].NextSpawnTime = Time.time + respawnTime;
    }

    private void FixedUpdate()
    {
        var time = Time.time;
        foreach (var i in plants)
        {
            if (i.IsPickedUp && i.NextSpawnTime <= time)
                SpawnPlant(i.CellPos.x, i.CellPos.y);
        }
    }

    private Vector3 GetPositionByCellPos(int x, int y)
    {
        return transform.position + new Vector3(
            x + halfCellSize.x,
            0,
            y + halfCellSize.y);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        var size = new Vector3(0.9f, 0, 0.9f);
        for (var x = 0; x < sizeX; x++)
        {
            for (var y = 0; y < sizeY; y++)
            {
                Gizmos.DrawWireCube(GetPositionByCellPos(x, y), size);
            }
        }
    }
}
