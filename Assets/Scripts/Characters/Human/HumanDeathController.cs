using Game;
using Items;
using Lobby;
using Unity.Netcode;
using UnityEngine;

namespace Characters.Human
{
    public class HumanDeathController : NetworkBehaviour
    {
        private PlayerSpawner _playerSpawner;
        private Inventory _inventory;
        private WeaponController _weaponController;
        
        private void Awake()
        {
            _playerSpawner = FindObjectOfType<PlayerSpawner>();
            _playerSpawner.OnPlayerDied += PlayerDied;
            _inventory = GetComponent<Inventory>();
            _weaponController = GetComponent<WeaponController>();
        }

        private void PlayerDied(Teams team, GameObject player, ulong playerId)
        {
            if (playerId == NetworkObject.OwnerClientId)
            {
                DropItemsServerRpc();
            }
        }

        [ServerRpc]
        private void DropItemsServerRpc()
        {
            foreach (var pocketItem in _inventory.PocketItems)
            {
                ItemDataManager.Instance.SpawnItem(pocketItem.itemType, pocketItem.count, transform.position,
                    Quaternion.identity);
            }

            ItemDataManager.Instance.SpawnItem(_inventory.CurrentHandItem.Value.itemType, 1, transform.position,
                Quaternion.identity);
            
            _weaponController.Drop();

            NetworkObject.Despawn(true);
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            _playerSpawner.OnPlayerDied -= PlayerDied;
        }
    }
}