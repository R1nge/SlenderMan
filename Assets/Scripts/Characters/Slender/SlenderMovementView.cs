using Unity.Netcode;
using UnityEngine;

namespace Characters.Slender
{
    [RequireComponent(typeof(CharacterController))]
    public class SlenderMovementView : NetworkBehaviour
    {
        [SerializeField] private float visibleSpeed = 5f;
        [SerializeField] private float invisibleSpeed = 14f;
        [SerializeField] private float gravity = 20f;
        private CharacterController _characterController;
        private SlenderMovement _slenderMovement;
        private SlenderVisibilityControllerView _slenderVisibility;

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
            _slenderMovement = new SlenderMovement(_characterController);
            _slenderMovement.SetVisibleSpeed(visibleSpeed);
            _slenderMovement.SetInvisibleSpeed(invisibleSpeed);
            _slenderMovement.SetGravity(gravity);
            _slenderVisibility = GetComponent<SlenderVisibilityControllerView>();
        }
        

        private void Update()
        {
            if (IsOwner)
            {
                _slenderMovement.Move(transform, _slenderVisibility.IsVisible().Value);
            }
        }
    }
}