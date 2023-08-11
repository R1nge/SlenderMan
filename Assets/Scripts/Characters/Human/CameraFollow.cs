using UnityEngine;

namespace Characters.Human
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] private Transform camera;
        [SerializeField] private Transform target;

        private void LateUpdate()
        {
            camera.position = target.position;
        }
    }
}