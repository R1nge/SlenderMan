using UnityEngine;

namespace Characters.Slender
{
    public class SlenderMovement
    {
        private readonly CharacterController _characterController;
        private Vector3 _moveDirection = Vector3.zero;
        private float _visibleSpeed, _invisibleSpeed;
        private float _gravity;

        public SlenderMovement(CharacterController characterController)
        {
            _characterController = characterController;
        }

        public void SetVisibleSpeed(float speed)
        {
            _visibleSpeed = speed;
        }

        public void SetInvisibleSpeed(float speed)
        {
            _invisibleSpeed = speed;
        }

        public void SetGravity(float gravity)
        {
            _gravity = gravity;
        }

        public void Move(Transform player, bool visible)
        {
            Vector3 forward = player.TransformDirection(Vector3.forward);
            Vector3 right = player.TransformDirection(Vector3.right);

            float curSpeedX = (visible ? _visibleSpeed : _invisibleSpeed) * Input.GetAxis("Vertical");
            float curSpeedY = (visible ? _visibleSpeed : _invisibleSpeed) * Input.GetAxis("Horizontal");
            _moveDirection = forward * curSpeedX + right * curSpeedY;


            if (!_characterController.isGrounded)
            {
                _moveDirection.y -= _gravity;
            }

            _characterController.Move(_moveDirection * Time.deltaTime);
        }
    }
}