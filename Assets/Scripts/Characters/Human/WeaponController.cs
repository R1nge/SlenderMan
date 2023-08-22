using Unity.Netcode;
using UnityEngine;

namespace Characters.Human
{
    public class WeaponController : NetworkBehaviour
    {
        [SerializeField] private Transform camera;
        [SerializeField] private Transform weaponHolder;
        [SerializeField] private float pickupDistance;
        private Shotgun _shotgun;

        private void Update()
        {
            if (!IsOwner) return;

            if (Input.GetKeyDown(KeyCode.E))
            {
                Ray ray = new Ray(camera.position, camera.forward);
                if (Physics.Raycast(ray, out var hit, pickupDistance))
                {
                    if (hit.transform.TryGetComponent(out Shotgun shotgun))
                    {
                        _shotgun = shotgun;
                        _shotgun.SetOwner(camera);
                        _shotgun.SetOwnerServerRpc(gameObject, NetworkObject.OwnerClientId);
                        SetOwnerServerRpc(_shotgun.gameObject);
                    }
                    else
                    {
                        Debug.LogError("Shotgun component is missing", this);
                    }
                }
            }

            if (Input.GetMouseButton(0))
            {
                if (_shotgun == null)
                {
                    Debug.LogError("Don't have a shotgun", this);
                    return;
                }

                _shotgun.ShootServerRpc();
            }
        }

        [ServerRpc]
        private void SetOwnerServerRpc(NetworkObjectReference shotgun)
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
        }
    }
}