using Characters.Slender;
using Game;
using Unity.Netcode;
using UnityEngine;

namespace Characters.Human
{
    public class Shotgun : NetworkBehaviour
    {
        [SerializeField] private float shootDistance;
        private Transform _camera;
        private PlayerSpawner _playerSpawner;

        private void Awake()
        {
            _playerSpawner = FindObjectOfType<PlayerSpawner>();
        }

        public void SetOwner(Transform camera)
        {
            _camera = camera;
        }

        [ServerRpc(RequireOwnership = false)]
        public void SetOwnerServerRpc(ulong ownerId)
        {
            NetworkObject.ChangeOwnership(ownerId);
        }

        [ServerRpc]
        public void ShootServerRpc()
        {
            Ray ray = new Ray(_camera.position, _camera.forward);
            if (Physics.Raycast(ray, out var hit, shootDistance))
            {
                if (hit.transform.TryGetComponent(out SlenderMovementView slender))
                {
                    if (slender.TryGetComponent(out NetworkObject net))
                    {
                        _playerSpawner.DeSpawn(net, net.OwnerClientId);
                    }
                    else
                    {
                        Debug.LogError("Slender component is missing", this);
                    }
                }
            }
        }
    }
}