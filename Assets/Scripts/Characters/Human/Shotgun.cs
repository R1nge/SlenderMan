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
        private Rigidbody _rigidbody;

        private void Awake()
        {
            _playerSpawner = FindObjectOfType<PlayerSpawner>();
            _rigidbody = GetComponent<Rigidbody>();
        }

        public void SetOwner(Transform camera)
        {
            _camera = camera;
        }

        [ServerRpc(RequireOwnership = false)]
        public void SetOwnerServerRpc(NetworkObjectReference player, ulong ownerId)
        {
            if (player.TryGet(out NetworkObject net))
            {
                NetworkObject.ChangeOwnership(ownerId);
                SetRigidbodyKinematic(true);
                SetKinematicClientRpc();

                if (NetworkObject.TrySetParent(net.transform))
                {
                    transform.localPosition = net.transform.GetChild(0).GetChild(1).localPosition;
                    transform.localRotation = Quaternion.Euler(new Vector3(0, 90, 0));
                    SetPositionClientRpc(player);
                }
                else
                {
                    Debug.LogError("Can't set shotgun parent", this);
                }
            }
            else
            {
                Debug.LogError("Network object is missing on a player", this);
            }
        }

        [ClientRpc]
        private void SetKinematicClientRpc()
        {
            SetRigidbodyKinematic(true);
        }

        [ClientRpc]
        private void SetPositionClientRpc(NetworkObjectReference player)
        {
            if (player.TryGet(out NetworkObject net))
            {
                transform.localPosition = net.transform.GetChild(0).GetChild(1).localPosition;
                transform.localRotation = Quaternion.Euler(new Vector3(0, 90, 0));
            }
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

        [ServerRpc(RequireOwnership = false)]
        public void DropServerRpc()
        {
            SetRigidbodyKinematic(false);
            DropClientRpc();
        }

        [ClientRpc]
        private void DropClientRpc()
        {
            SetRigidbodyKinematic(false);
        }

        private void SetRigidbodyKinematic(bool kinematic) => _rigidbody.isKinematic = kinematic;
    }
}