using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private Joystick joystick;
        private CharacterController controller;
        
        public float movementSpeed = 3f;
        private const float Gravity = 10f;
        
        private Vector3 inputDirNormalized;
        public bool IsRunning { get; private set; }
        
        
        private static readonly int IsRunAnim = Animator.StringToHash("isRun");
        private static readonly int AttackAnim = Animator.StringToHash("attack");

        private void Start()
        {
            controller = GetComponent<CharacterController>();
        }

        private void Update()
        {
            inputDirNormalized = joystick.Direction.normalized;
            IsRunning = inputDirNormalized != Vector3.zero;

            UpdateMove();
            
            if (!IsRunning)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    animator.SetTrigger(AttackAnim);
                }
            }
        }

        private void UpdateMove()
        {
            animator.SetBool(IsRunAnim, IsRunning);
            
            var dt = Time.deltaTime;
            controller.Move(Vector3.down * (Gravity * dt));
            
            if (!IsRunning) return;
            
            animator.SetBool(IsRunAnim, IsRunning);
            var move = new Vector3(inputDirNormalized.x, 0, inputDirNormalized.y) * (movementSpeed * dt);
            transform.rotation = Quaternion.LookRotation(move, Vector3.up);
            controller.Move(move);   
        }
    }
}