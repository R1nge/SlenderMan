using Characters.Human;
using Characters.Human.Pickup;
using Unity.Netcode;
using UnityEngine;

namespace Items
{
    public class ItemView : NetworkBehaviour, IPickupable
    {
        [SerializeField] private Item.ItemType item;
        private Item _item;

        private void Awake()
        {
            _item = new Item(item);
        }

        public void Pickup(Inventory inventory)
        {
            inventory.Add(_item);
            DestroyServerRpc();
        }

        [ServerRpc(RequireOwnership = false)]
        private void DestroyServerRpc()
        {
            NetworkObject.Despawn(true);
        }
    }
}