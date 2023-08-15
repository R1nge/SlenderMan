using System.Collections.Generic;
using Items;
using Unity.Netcode;
using UnityEngine;

namespace Characters.Human
{
    public class Inventory : NetworkBehaviour
    {
        private Dictionary<Item.ItemType, Item> _items = new();
        private Item.ItemType _currentItem;
        private int _index;

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

            if (_items.Count == 0) return;

            var delta = Input.GetAxis("Mouse ScrollWheel") * 10;
            if (delta >= 1)
            {
                _index = (_index + 1) % _items.Count;
                _currentItem = _items[(Item.ItemType)_index].Type;
            }
            else if (delta <= -1)
            {
                _index = (_index - 1) % _items.Count;
                _currentItem = _items[(Item.ItemType)_index].Type;
            }
        }

        [ServerRpc(RequireOwnership = false)]
        private void DropServerRpc()
        {
            if (_items.ContainsKey(_currentItem))
            {
                print($"TRYING TO DROP {_currentItem}");
                ItemSpawner.Instance.Spawn(_currentItem, _items[_currentItem].Count,
                    transform.position);
                _items[_currentItem].Count--;
                print(_items[_currentItem].Count);

                if (_items[_currentItem].Count < 0)
                {
                    _items[_currentItem].Count = 0;
                }

                if (_items[_currentItem].Count == 0)
                {
                    print(_items[_currentItem].Count);
                    Remove(_currentItem);
                    print(_items.ContainsKey(_currentItem));
                }
            }
        }
    }
}