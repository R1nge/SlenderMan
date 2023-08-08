using Unity.Netcode;
using UnityEngine;

namespace Characters.Human
{
    [RequireComponent(typeof(CharacterController))]
    public class HumanMovementView : NetworkBehaviour
    {
        [SerializeField] private float walkingSpeed = 7.5f;
        [SerializeField] private float runningSpeed = 11.5f;
        [SerializeField] private float gravity = 20f;
        private CharacterController _characterController;
        private HumanMovement _humanMovement;

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
            _humanMovement = new HumanMovement(_characterController);
            _humanMovement.SetWalkingSpeed(walkingSpeed);
            _humanMovement.SetRunningSpeed(runningSpeed);
            _humanMovement.SetGravity(gravity);
        }

        private void Update()
        {
            if (IsOwner)
            {
                var isRunning = Input.GetKey(KeyCode.LeftShift);
                _humanMovement.Move(transform, isRunning);
            }
        }
    }
}