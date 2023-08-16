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

        public void Remove(Item item, uint amount)
        {
            for (int i = 0; i < _items.Count; i++)
            {
                if (_items[i].Type == item.Type)
                {
                    var item1 = _items[i];
                    item1.Count -= amount;

                    if (item1.Count == 0)
                    {
                        RemoveAt(i);
                        print("REMOVED");
                    }
                    else
                    {
                        _items[i] = item1;
                    }
                }
            }
        }

        public void RemoveAt(int index)
        {
            _items.RemoveAt(index);
        }

        public bool HasItem(Item item, uint amount)
        {
            for (int i = 0; i < _items.Count; i++)
            {
                if (_items[i].Type == item.Type)
                {
                    if (_items[i].Count >= amount)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private void Update()
        {
            if (!IsOwner) return;

            if (_items.Count == 0)
            {
                _index = 0;
                return;
            }

            if (Input.GetKeyDown(KeyCode.G))
            {
                DropServerRpc();
                return;
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
            if (_items.Contains(_currentItem.Value))
            {
                ItemData.Instance.Spawn(_currentItem.Value.Type, _items[_index].Count, transform.position);
                var currentItemValue = _currentItem.Value;
                currentItemValue.Count = 0;
                _items[_index] = currentItemValue;
                Debug.LogError($"TRYING TO DROP {_currentItem.Value.Type}");

                if (_items[_index].Count == 0)
                {
                    print(_items[_index].Count);
                    RemoveAt(_index);
                    _currentItem = new NetworkVariable<Item>();
                }
            }
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            _items?.Dispose();
        }
    }
}