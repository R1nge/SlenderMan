using Unity.Netcode;
using UnityEngine;

namespace Characters.Human
{
    [RequireComponent(typeof(CharacterController))]
    public class HumanMovementView : NetworkBehaviour
    {
        [SerializeField] private float crouchingSpeed = 6f;
        [SerializeField] private float walkingSpeed = 7.5f;
        [SerializeField] private float runningSpeed = 11.5f;
        [SerializeField] private float gravity = 20f;
        private CharacterController _characterController;
        private HumanAnimationController _animator;
        private HumanMovement _humanMovement;
        private bool _isCrouching;
        private const float BasicHeight = 1.91f;
        private const float CrouchHeight = 1f;

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
            _animator = GetComponent<HumanAnimationController>();
            _humanMovement = new HumanMovement(_characterController, _animator);
            _humanMovement.SetCrouchSpeed(crouchingSpeed);
            _humanMovement.SetWalkingSpeed(walkingSpeed);
            _humanMovement.SetRunningSpeed(runningSpeed);
            _humanMovement.SetGravity(gravity);
        }

        private void Update()
        {
            if (IsOwner)
            {
                var isRunning = Input.GetKey(KeyCode.LeftShift);

                if (Input.GetKeyDown(KeyCode.LeftControl))
                {
                    _isCrouching = !_isCrouching;
                }

                if (_isCrouching)
                {
                    _characterController.height = CrouchHeight;
                }
                else
                {
                    _characterController.height = BasicHeight;
                }

                _humanMovement.Move(transform, isRunning, _isCrouching);
            }
        }
    }
}