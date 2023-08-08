using UnityEngine;

namespace Characters
{
    public class CameraController
    {
        private (float, float) _limits;
        private float _sensitivity;
        private float _rotationX;

        public void SetLimits((float, float) limits)
        {
            _limits = limits;
        }
        
        public void SetSensitivity(float sensitivity)
        {
            _sensitivity = sensitivity;
        }

        public void Rotate(Transform player, Transform camera)
        {
            _rotationX += -Input.GetAxis("Mouse Y") * _sensitivity;
            _rotationX = Mathf.Clamp(_rotationX, _limits.Item1, _limits.Item2);
            camera.localRotation = Quaternion.Euler(_rotationX, 0, 0);
            player.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * _sensitivity, 0);
        }
    }
}