using System;
using Characters.Human.Interact;
using Characters.Human.Pickup;
using Unity.Netcode;
using UnityEngine;

namespace Characters.Human
{
    public class WeaponInventoryBridge : NetworkBehaviour
    {
        [SerializeField] private Transform camera;
        [SerializeField] private float pickupDistance;
        private Inventory _inventory;
        private WeaponController _weaponController;

        private void Awake()
        {
            _inventory = GetComponent<Inventory>();
            _inventory.OnHandItemSwapped += Swap;
            _weaponController = GetComponent<WeaponController>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Ray ray = new Ray(camera.position, camera.forward);
                if (Physics.Raycast(ray, out var hit, pickupDistance))
                {
                    if (hit.transform.TryGetComponent(out IInteractable intractable))
                    {
                        intractable.Interact(_inventory);
                        return;
                    }

                    if (hit.transform.TryGetComponent(out Pickupable pickupable))
                    {
                        if (pickupable.HasOwner)
                        {
                            Debug.LogError("Can't pickup, someone already is holding it");
                            return;
                        }

                        pickupable.Pickup(_inventory);
                        
                        if (pickupable.transform.TryGetComponent(out Shotgun shotgun))
                        {
                            shotgun.SetOwner(camera);
                            shotgun.SetOwnerServerRpc(gameObject, NetworkObject.OwnerClientId);
                            _weaponController.SetOwnerServerRpc(shotgun.gameObject);
                        }
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.H))
            {
                Drop(_inventory.CurrentHandItem.Value);
                Debug.LogError("Drop hand item");
            }
        }

        private void Drop(Item item)
        {
            if (_weaponController.HasWeapon())
            {
                _weaponController.Drop();
                return;
            }

            if (_inventory.CurrentHandItem.Value.itemType != Item.ItemType.None)
            {
                _inventory.DropServerRpc(item);
            }
        }

        private void Swap(Item item)
        {
            if (_inventory.CurrentHandItem.Value.itemType != Item.ItemType.None)
            {
                if (_inventory.CurrentHandItem.Value.itemType == Item.ItemType.Shotgun)
                {
                    if (_weaponController.HasWeapon())
                    {
                        _weaponController.Drop();
                    }
                }

                _inventory.DropServerRpc(_inventory.CurrentHandItem.Value);
            }
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            _inventory.OnHandItemSwapped -= Swap;
        }
    }
}