using UnityEngine;

namespace Player
{
    public class AttackHandler : MonoBehaviour
    {
        [SerializeField] private Vector3 attackCenter;
        [SerializeField] private float attackRadius = 1f;
        [Space] 
        [SerializeField] private GameObject weapon;

        private readonly Collider[] results = new Collider[10];

        private void Start()
        {
            SetWeaponVisible(false);
        }

        public void SetWeaponVisible(bool value) => weapon.SetActive(value);
        
        // animation event method
        private void HandleAttack()
        {
            var boxPosition = transform.position + transform.TransformDirection(attackCenter);
            var count = Physics.OverlapSphereNonAlloc(boxPosition, attackRadius, results);
            for (var i = 0; i < count; i++)
            {
                if (!results[i].TryGetComponent(out Plant plant)) continue;
                plant.CutOff();
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            var boxPosition = transform.position + transform.TransformDirection(attackCenter); 
            Gizmos.DrawWireSphere(boxPosition, attackRadius);
        }
    }
}
