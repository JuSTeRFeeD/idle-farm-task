using UnityEngine;

namespace Scriptable
{
    [CreateAssetMenu(menuName = "Plants/plant")]
    public class PlantData : ScriptableObject
    {
        public GameObject stackPrefab;
        public string title;
        [Min(1)] public int costPerStackItem;
    }
}
