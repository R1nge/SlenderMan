using Unity.Netcode;
using UnityEngine;

namespace Characters
{
    public class CameraControllerView : NetworkBehaviour
    {
        [SerializeField] private float minLimit, maxLimit;
        [SerializeField] private float sensitivity;
        [SerializeField] private Camera playerCamera;
        [SerializeField] private AudioListener audioListener;
        private CameraController _cameraController;

        private void Awake()
        {
            _cameraController = new CameraController();
            _cameraController.SetLimits((minLimit, maxLimit));
            _cameraController.SetSensitivity(sensitivity);
        }

        public override void OnNetworkSpawn()
        {
            playerCamera.enabled = NetworkObject.IsOwner;
            audioListener.enabled = NetworkObject.IsOwner;
        }

        private void Update()
        {
            if (NetworkObject.IsOwner)
            {
                _cameraController.Rotate(transform, playerCamera.transform);
            }
        }
    }
}