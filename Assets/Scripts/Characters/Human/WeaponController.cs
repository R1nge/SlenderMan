using Unity.Netcode;
using UnityEngine;

namespace Characters.Human
{
    public class WeaponController : NetworkBehaviour
    {
        [SerializeField] private Transform camera;
        private Shotgun _shotgun;

        public bool HasWeapon() => _shotgun != null;

        private void Update()
        {
            if (!IsOwner) return;

            if (Input.GetMouseButton(0))
            {
                if (HasWeapon())
                {
                    _shotgun.ShootServerRpc();
                }
                else
                {
                    Debug.LogError("Don't have a shotgun", this);
                }
            }
        }

        [ServerRpc]
        public void SetOwnerServerRpc(NetworkObjectReference shotgun)
        {
            if (shotgun.TryGet(out NetworkObject net))
            {
                _shotgun = net.GetComponent<Shotgun>();
                _shotgun.SetOwner(camera);
                SetOwnerClientRpc(shotgun);
            }
            else
            {
                Debug.LogError("Shotgun missing a network object", this);
            }
        }

        [ClientRpc]
        private void SetOwnerClientRpc(NetworkObjectReference shotgun)
        {
            if (shotgun.TryGet(out NetworkObject net))
            {
                _shotgun = net.GetComponent<Shotgun>();
                _shotgun.SetOwner(camera);
            }
            else
            {
                Debug.LogError("Shotgun missing a network object", this);
            }
        }

        public void Drop()
        {
            _shotgun.SetOwner(null);
            _shotgun.GetComponent<NetworkObject>().TryRemoveParent();
            _shotgun.DropServerRpc();
            _shotgun = null;
            print("DROPPED SHOTGUN");
        }
    }
}