using UnityEngine;

internal struct PlantData
{
    public bool IsPickedUp;
    public float NextSpawnTime;
}

public class GardenBed : MonoBehaviour
{
    [SerializeField] private Plant plantPrefab;
    [SerializeField] private float respawnTime = 10f;
    [Space]
    [SerializeField] private int sizeX = 10;
    [SerializeField] private int sizeY = 7;

    private PlantData[] plants;

    private readonly Vector3 halfCellSize = new Vector3(0.5f, 0, 0.5f);
    
    private void Start()
    {
        plants = new PlantData[sizeX * sizeY];
        for (var i = 0; i < plants.Length; i++)
        {
            SpawnPlant(i);
        }
    }

    private void SpawnPlant(int index) 
    {
        // TODO: pool objects
        var plant = Instantiate(plantPrefab, GetPlantPositionByIndex(index), Quaternion.identity, transform);
        plants[index] = new PlantData();
        plant.Setup(index);
        plant.OnPickup += HandlePickupPlant;
    }

    private void HandlePickupPlant(int plantIndex)
    {
        plants[plantIndex].IsPickedUp = true;
        plants[plantIndex].NextSpawnTime = Time.time + respawnTime;
    }

    private void FixedUpdate()
    {
        var time = Time.time;
        for (var i = 0; i < plants.Length; i++)
        {
            if (plants[i].IsPickedUp && plants[i].NextSpawnTime <= time)
            {
                SpawnPlant(i);
            }
        }
    }

    private Vector3 GetPlantPositionByIndex(int index)
    {
        return transform.position + new Vector3(
            index % sizeX  + halfCellSize.x,
            0,
            index % sizeY + halfCellSize.y);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.grey;
        var size = new Vector3(0.9f, 0, 0.9f);
        for (var x = 0; x < sizeX; x++)
        {
            for (var y = 0; y < sizeY; y++)
            {
                Gizmos.DrawWireCube(transform.position + new Vector3(x + halfCellSize.x, 0, y + halfCellSize.y), size);
            }
        }
    }
}
