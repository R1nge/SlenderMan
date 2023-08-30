using Characters.Human;
using Characters.Human.Pickup;
using Unity.Netcode;
using UnityEngine;

namespace Items
{
    public class ItemView : Pickupable
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


        public override void Pickup(Inventory inventory)
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
                    SetOwner();
                    SetOwnerClientRpc();
                    if (spawnModelOnPickup)
                    {
                        Destroy();
                    }
                }
            }
        }

        [ClientRpc]
        private void SetOwnerClientRpc() => SetOwner();

        private void Destroy()
        {
            NetworkObject.Despawn(true);
        }
    }
}