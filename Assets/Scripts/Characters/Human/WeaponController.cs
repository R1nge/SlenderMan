﻿using Unity.Netcode;
using UnityEngine;

namespace Characters.Human
{
    public class WeaponController : NetworkBehaviour
    {
        [SerializeField] private Transform camera;
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
                        _shotgun.SetOwnerServerRpc(NetworkObject, NetworkObject.OwnerClientId);
                    }
                    else
                    {
                        Debug.LogError("Shotgun component is missing", this);
                    }
                }
            }

            if (_shotgun == null)
            {
                Debug.LogError("Don't have a shotgun", this);
                return;
            }

            if (Input.GetMouseButton(0))
            {
                _shotgun.ShootServerRpc();
            }
        }
    }
}