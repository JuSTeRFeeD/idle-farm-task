using System;
using DG.Tweening;
using UnityEngine;

namespace Player
{
    public class PlayerBackpack : MonoBehaviour
    {
        [SerializeField] private PlayerController playerController;
        [SerializeField, Tooltip("From this position plants will be display behind player")] 
        private Transform backpackStackContainer;

        [field: SerializeField] public int StackSize { get; private set; } = 40;
        public int CurrentCollectedCount { get; private set; }
        public bool HasItems => CurrentCollectedCount > 0;
        public bool IsFull => CurrentCollectedCount >= StackSize;
        public Plant[] CollectedPlants { get; private set; }

        [Header("Pickup")] 
        [SerializeField] private float pickupDistance = 2f; 
        [SerializeField] private float distanceBetweenItems = 0.2f;
        [SerializeField] private float pickupTime = 1f;
        [SerializeField] private float pickedScale = 0.3f;
        [Header("Wiggle")]
        [SerializeField] private float wiggleDistance = 0.4f;
        [SerializeField] private float wiggleSpeed = 10;
        
        public event Action OnCollectedChange;
        public event Action OnBackpackFull;

        private void Start()
        {
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

        private Collider[] collidedPlants = new Collider[10];
        private void FixedUpdate()
        {
            var count = Physics.OverlapSphereNonAlloc(transform.position, pickupDistance, collidedPlants);
            for (var i = 0; i < count; i++)
            {
                if (collidedPlants[i].TryGetComponent(out Plant plant))
                {
                    plant.Pickup(this);
                }
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, pickupDistance);
        }

        /// <returns>True - item was added</returns>
        public bool AddItem(Plant plant)
        {
            if (CurrentCollectedCount >= StackSize) return false;
            
            var containerPos = backpackStackContainer.localPosition;
            var path = new Vector3[]
            {
                containerPos + Vector3.up + Vector3.up * (distanceBetweenItems * CurrentCollectedCount),
                containerPos + Vector3.up * (distanceBetweenItems * CurrentCollectedCount)
            };
            plant.transform.DOScale(pickedScale, pickupTime / 2);
            plant.transform.SetParent(backpackStackContainer);
            plant.transform.DOLocalPath(path, pickupTime);
            plant.transform.DOLocalRotate(Vector3.zero, pickupTime);
            
            CollectedPlants[CurrentCollectedCount] = plant;
            CurrentCollectedCount++;

            if (CurrentCollectedCount >= StackSize)
            {
                OnBackpackFull?.Invoke();
            }
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
