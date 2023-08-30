using Characters.Human;
using Characters.Human.Pickup;
using Unity.Netcode;
using UnityEngine;

namespace Items
{
    public class ItemView : NetworkBehaviour, IPickupable
    {
        [SerializeField] private Item.EquipType equipType;
        [SerializeField] private Item.ItemType itemType;
        [SerializeField] private uint count;
        [SerializeField] private bool spawnModelOnPickup;
        private Item _item;

        private void Awake()
        {
            _item = new Item(equipType, itemType, count, spawnModelOnPickup);
        }

        public void SetCount(uint newCount) => count = newCount;

        public void Pickup(Inventory inventory)
        {
            AddServerRpc(inventory.gameObject);
        }

        [ServerRpc(RequireOwnership = false)]
        private void AddServerRpc(NetworkObjectReference player)
        {
            if (player.TryGet(out NetworkObject playerGo))
            {
                if (playerGo.GetComponent<Inventory>().Add(_item))
                {
                    if (spawnModelOnPickup)
                    {
                        Destroy();
                    }
                }
            }
        }

        private void Destroy()
        {
            NetworkObject.Despawn(true);
        }
    }
}