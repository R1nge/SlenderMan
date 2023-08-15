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
            _item = new Item(item, 1);
        }

        public void Pickup(Inventory inventory)
        {
            AddServerRpc(inventory.gameObject);
            DestroyServerRpc();
        }

        [ServerRpc(RequireOwnership = false)]
        private void AddServerRpc(NetworkObjectReference player)
        {
            if (player.TryGet(out NetworkObject playerGo))
            {
                playerGo.GetComponent<Inventory>().Add(_item);
            }
        }

        [ServerRpc(RequireOwnership = false)]
        private void DestroyServerRpc()
        {
            NetworkObject.Despawn(true);
        }
    }
}