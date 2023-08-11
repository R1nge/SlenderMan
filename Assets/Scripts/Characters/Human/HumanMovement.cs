using UnityEngine;

namespace Characters.Human
{
    public class HumanMovement
    {
        private readonly CharacterController _characterController;
        private readonly HumanAnimationController _animator;
        private Vector3 _moveDirection = Vector3.zero;
        private float _walkingSpeed, _runningSpeed;
        private float _gravity;

        public HumanMovement(CharacterController characterController, HumanAnimationController animator)
        {
            _characterController = characterController;
            _animator = animator;
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

            float curSpeedX = (run ? _runningSpeed : _walkingSpeed) * Input.GetAxis("Vertical");
            float curSpeedY = (run ? _runningSpeed : _walkingSpeed) * Input.GetAxis("Horizontal");
            _moveDirection = forward * curSpeedX + right * curSpeedY;

            var speed = Mathf.Abs(curSpeedX) + Mathf.Abs(curSpeedY) / _runningSpeed;
            speed = Mathf.Clamp01(speed);
            _animator.SetSpeed(speed);
            _animator.SetDirection(new Vector2(Input.GetAxis("Vertical"), Input.GetAxis("Horizontal")));

            if (!_characterController.isGrounded)
            {
                _moveDirection.y -= _gravity;
            }

            _characterController.Move(_moveDirection * Time.deltaTime);
        }
    }
}