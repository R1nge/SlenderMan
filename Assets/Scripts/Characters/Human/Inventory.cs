using System;
using System.Collections.Generic;
using Items;
using Unity.Netcode;
using UnityEngine;

namespace Characters.Human
{
    public class Inventory : NetworkBehaviour
    {
        private NetworkList<Item> _items;
        private NetworkVariable<Item> _currentItem;
        private int _index;

        public NetworkList<Item> Items => _items;
        public NetworkVariable<Item> CurrentItem => _currentItem;

        private void Awake()
        {
            _items = new();
            _currentItem = new();
        }

        public void Add(Item item)
        {
            _items.Add(item);
        }

        public void Remove(Item item)
        {
            _items.Remove(item);
        }

        public bool HasItem(Item item)
        {
            return _items.Contains(item);
        }

        private void Update()
        {
            if (!IsOwner) return;

            if (_items.Count == 0) return;

            if (Input.GetKeyDown(KeyCode.G))
            {
                DropServerRpc();
            }

            var delta = Input.GetAxis("Mouse ScrollWheel") * 10;
            if (delta >= 1)
            {
                ChangeForwardServerRpc();
            }
            else if (delta <= -1)
            {
                ChangeBackwardServerRpc();
            }
        }

        [ServerRpc(RequireOwnership = false)]
        private void ChangeForwardServerRpc()
        {
            _index = (_index + 1) % _items.Count;
            _currentItem.Value = _items[_index];
            Debug.LogError(_currentItem.Value.Type);
        }

        [ServerRpc(RequireOwnership = false)]
        private void ChangeBackwardServerRpc()
        {
            _index = Mathf.Clamp((_index - 1) % _items.Count, 0, _items.Count);
            _currentItem.Value = _items[_index];
            Debug.LogError(_currentItem.Value.Type);
        }

        [ServerRpc(RequireOwnership = false)]
        private void DropServerRpc()
        {
            Debug.LogError($"TRYING TO DROP {_currentItem.Value.Type}");
            ItemSpawner.Instance.Spawn(_currentItem.Value.Type, _items[_index].Count, transform.position);
            var item = _items[_index];
            item.Count--;
            print(item.Count);

            if (item.Count == 0)
            {
                print(item.Count);
                Remove(_currentItem.Value);
                print(_items.Contains(_currentItem.Value));
            }
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            _items?.Dispose();
        }
    }
}