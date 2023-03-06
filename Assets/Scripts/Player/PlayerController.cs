using UnityEngine;
using Input;

namespace Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private PlayerBackpack backpack;
        [SerializeField] private Animator animator;
        [SerializeField] private Input.Joystick joystick;
        private CharacterController controller;
        
        public float movementSpeed = 3f;
        private const float Gravity = 10f;
        
        public bool IsRunning { get; private set; }
        
        private static readonly int IsRunAnim = Animator.StringToHash("isRun");
        private static readonly int AttackAnim = Animator.StringToHash("attack");

        private void Start()
        {
            controller = GetComponent<CharacterController>();
            backpack.OnBackpackFull += () => animator.SetBool(AttackAnim, false);
        }


        private void Update()
        {
            var inputDir = joystick.Direction.normalized;
            IsRunning = inputDir != Vector2.zero;

            var dt = Time.deltaTime;
            controller.Move(Vector3.down * (Gravity * dt));
            animator.SetBool(IsRunAnim, IsRunning);
            
            if (!IsRunning) return;
            
            var move = new Vector3(inputDir.x, 0, inputDir.y) * (movementSpeed * dt);
            transform.rotation = Quaternion.LookRotation(move, Vector3.up);
            controller.Move(move);  
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!backpack.IsFull && other.isTrigger && other.CompareTag("GardenZone"))
            {
                animator.SetBool(AttackAnim, true);
            }
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (other.isTrigger && other.CompareTag("GardenZone"))
            {
                animator.SetBool(AttackAnim, false);
            }
        }
    }
}