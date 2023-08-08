using UnityEngine;

namespace Characters.Human
{
    public class HumanMovement
    {
        private readonly CharacterController _characterController;
        private Vector3 _moveDirection = Vector3.zero;
        private float _walkingSpeed, _runningSpeed;
        private float _gravity;

        public HumanMovement(CharacterController characterController)
        {
            _characterController = characterController;
        }

        public void SetWalkingSpeed(float speed)
        {
            _walkingSpeed = speed;
        }

        public void SetRunningSpeed(float speed)
        {
            _runningSpeed = speed;
        }

        public void SetGravity(float gravity)
        {
            _gravity = gravity;
        }

        public void Move(Transform player, bool run)
        {
            Vector3 forward = player.TransformDirection(Vector3.forward);
            Vector3 right = player.TransformDirection(Vector3.right);

            float curSpeedX = run ? _runningSpeed : _walkingSpeed * Input.GetAxis("Vertical");
            float curSpeedY = run ? _runningSpeed : _walkingSpeed * Input.GetAxis("Horizontal");
            _moveDirection = forward * curSpeedX + right * curSpeedY;


            if (!_characterController.isGrounded)
            {
                _moveDirection.y -= _gravity;
            }

            _characterController.Move(_moveDirection * Time.deltaTime);
        }
    }
}