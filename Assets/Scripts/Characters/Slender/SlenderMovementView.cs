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
        private bool _visible = true;
        private CharacterController _characterController;
        private SlenderMovement _slenderMovement;

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
            _slenderMovement = new SlenderMovement(_characterController);
            _slenderMovement.SetVisibleSpeed(visibleSpeed);
            _slenderMovement.SetInvisibleSpeed(invisibleSpeed);
            _slenderMovement.SetGravity(gravity);
        }

        private void Update()
        {
            if (IsOwner)
            {
                if (Input.GetKeyDown(KeyCode.V))
                {
                    _visible = !_visible;
                }

                _slenderMovement.Move(transform, _visible);
            }
        }
    }
}