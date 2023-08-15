using System.Collections.Generic;
using Items;
using Unity.Netcode;
using UnityEngine;

namespace Characters.Human
{
    public class Inventory : NetworkBehaviour
    {
        private Dictionary<Item.ItemType, Item> _items = new();

        public void Add(Item item)
        {
            _items.Add(item.Type, item);
        }

        public void Remove(Item.ItemType itemType)
        {
            _items.Remove(itemType);
        }

        public bool HasItem(Item.ItemType type) => _items.ContainsKey(type);

        private void Update()
        {
            if (!IsOwner) return;
            if (Input.GetKeyDown(KeyCode.G))
            {
                DropServerRpc();
            }
        }

        [ServerRpc(RequireOwnership = false)]
        private void DropServerRpc()
        {
            print("DROP");
            if (_items.ContainsKey(Item.ItemType.Gasoline))
            {
                print("DROP GASOLINE");
                ItemSpawner.Instance.Spawn(Item.ItemType.Gasoline, _items[Item.ItemType.Gasoline].Count,
                    transform.position);
                _items[Item.ItemType.Gasoline].Count--;
                print(_items[Item.ItemType.Gasoline].Count);

                if (_items[Item.ItemType.Gasoline].Count < 0)
                {
                    _items[Item.ItemType.Gasoline].Count = 0;
                }

                if (_items[Item.ItemType.Gasoline].Count == 0)
                {
                    Remove(Item.ItemType.Gasoline);
                    print(_items.ContainsKey(Item.ItemType.Gasoline));
                }
            }
        }
    }
}