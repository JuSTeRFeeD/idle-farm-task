using System;
using DG.Tweening;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(PlayerController))]
    public class PlayerBackpack : MonoBehaviour
    {
        [SerializeField, Tooltip("From this position plants will be display behind player")] 
        private Transform backpackStackContainer;

        private PlayerController playerController;

        [SerializeField] private float plantStackHeight = 0.2f;
        [field: SerializeField] public int StackSize { get; private set; } = 40;
        public int CurrentCollectedCount { get; private set; }
        public bool HasItems => CurrentCollectedCount > 0;
        public Plant[] CollectedPlants { get; private set; }
        
        [Space]
        [SerializeField] private float wiggleDistance = 0.4f;
        [SerializeField] private float wiggleSpeed = 10;
        
        public event Action OnCollectedChange;

        private void Start()
        {
            playerController = GetComponent<PlayerController>();
            CollectedPlants = new Plant[StackSize];
        }

        private void Update()
        {
            if (!playerController.IsRunning)
            {
                backpackStackContainer.localRotation = Quaternion.Lerp(
                    backpackStackContainer.localRotation, 
                    Quaternion.Euler(Vector3.zero),
                    wiggleSpeed * Time.deltaTime);
            }
            else
            {
                var wiggleValue = Mathf.Sin(Time.time * wiggleSpeed) * wiggleDistance;
                backpackStackContainer.localRotation = Quaternion.Euler(new Vector3(0, 0, wiggleValue));
            }
        }

        /// <returns>True - item was added</returns>
        public bool AddItem(Plant plant)
        {
            if (CurrentCollectedCount >= StackSize) return false;
            
            var containerPos = backpackStackContainer.localPosition;
            var path = new Vector3[]
            {
                containerPos + Vector3.up * 2f  + Vector3.up * plantStackHeight * CurrentCollectedCount,
                containerPos + Vector3.up * plantStackHeight * CurrentCollectedCount
            };
            plant.transform.SetParent(backpackStackContainer);
            plant.transform.DOLocalPath(path, 1f);
            plant.transform.DOLocalRotate(Vector3.zero, 1f);
            
            CollectedPlants[CurrentCollectedCount] = plant;
            CurrentCollectedCount++;
            OnCollectedChange?.Invoke();
            return true;
        }

        public Plant PopItem()
        {
            var plant = CollectedPlants[CurrentCollectedCount - 1];
            plant.transform.SetParent(null);
            CurrentCollectedCount--;
            OnCollectedChange?.Invoke();
            return plant;
        }
    }
}
