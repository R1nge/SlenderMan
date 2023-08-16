using Items;
using Unity.Netcode;
using UnityEngine;

namespace Characters.Human
{
    public class Inventory : NetworkBehaviour
    {
        [SerializeField] private int maxSize;
        [SerializeField] private Hand hand;
        private NetworkList<Item> _pocketItems;
        private NetworkVariable<Item> _currentPocketItem;
        private NetworkVariable<Item> _handItem;
        private int _index;

        public NetworkList<Item> PocketItems => _pocketItems;
        public NetworkVariable<Item> CurrentPocketItem => _currentPocketItem;
        public NetworkVariable<Item> CurrentHandItem => _handItem;

        private void Awake()
        {
            _pocketItems = new();
            _currentPocketItem = new();
            _handItem = new();
        }

        public bool Add(Item item)
        {
            if (item.equipType == Item.EquipType.Hand)
            {
                if (_handItem.Value.itemType == item.itemType)
                {
                    Debug.LogError("Hand is full");
                    return false;
                }

                _handItem.Value = item;
                var net = ItemData.Instance.SpawnModel(item.itemType, hand.transform.position, Quaternion.identity);
                hand.SetChild(net.transform);
                return true;
            }

            if (item.equipType == Item.EquipType.Pocket)
            {
                if (_pocketItems.Count < maxSize)
                {
                    _pocketItems.Add(item);
                    return true;
                }

                Debug.LogError("Pockets are full. Can't add new item.");
                return false;
            }

            return false;
        }

        public void Remove(Item item, uint amount)
        {
            if (item.equipType == Item.EquipType.Pocket)
            {
                for (int i = 0; i < _pocketItems.Count; i++)
                {
                    if (_pocketItems[i].itemType == item.itemType)
                    {
                        var item1 = _pocketItems[i];
                        item1.count -= amount;

                        if (item1.count == 0)
                        {
                            RemoveAt(i);
                            print("REMOVED");
                        }
                        else
                        {
                            _pocketItems[i] = item1;
                        }
                    }
                }
            }
            else if (item.equipType == Item.EquipType.Hand)
            {
                if (_handItem.Value.itemType == item.itemType)
                {
                    if (_handItem.Value.count >= item.count)
                    {
                        hand.DestroyChild();
                        _handItem = new NetworkVariable<Item>();
                    }
                }
            }
        }

        public void RemoveAt(int index)
        {
            _pocketItems.RemoveAt(index);
        }

        public bool HasItem(Item item, uint amount)
        {
            if (item.equipType == Item.EquipType.Pocket)
            {
                for (int i = 0; i < _pocketItems.Count; i++)
                {
                    if (_pocketItems[i].itemType == item.itemType)
                    {
                        if (_pocketItems[i].count >= amount)
                        {
                            return true;
                        }
                    }
                }

                return false;
            }

            if (item.equipType == Item.EquipType.Hand)
            {
                if (_handItem.Value.itemType == item.itemType)
                {
                    if (_handItem.Value.count >= amount)
                    {
                        return true;
                    }
                }

                return false;
            }


            return false;
        }

        private void Update()
        {
            if (!IsOwner) return;

            if (Input.GetKeyDown(KeyCode.H))
            {
                DropServerRpc(_handItem.Value);
                Debug.LogError("Drop hand item");
                return;
            }

            if (_pocketItems.Count == 0)
            {
                _index = 0;
                return;
            }

            if (Input.GetKeyDown(KeyCode.G))
            {
                DropServerRpc(_currentPocketItem.Value);
                Debug.LogError("Drop pocket item");
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
            _index = (_index + 1) % _pocketItems.Count;
            _currentPocketItem.Value = _pocketItems[_index];
            Debug.LogError(_currentPocketItem.Value.itemType);
        }

        [ServerRpc(RequireOwnership = false)]
        private void ChangeBackwardServerRpc()
        {
            _index = Mathf.Clamp((_index - 1) % _pocketItems.Count, 0, _pocketItems.Count);
            _currentPocketItem.Value = _pocketItems[_index];
            Debug.LogError(_currentPocketItem.Value.itemType);
        }

        [ServerRpc(RequireOwnership = false)]
        private void DropServerRpc(Item item)
        {
            if (item.equipType == Item.EquipType.Pocket)
            {
                if (_pocketItems.Contains(_currentPocketItem.Value))
                {
                    ItemData.Instance.SpawnItem(_currentPocketItem.Value.itemType, _pocketItems[_index].count,
                        transform.position, Quaternion.identity);
                    var currentItemValue = _currentPocketItem.Value;
                    currentItemValue.count = 0;
                    _pocketItems[_index] = currentItemValue;
                    Debug.LogError($"TRYING TO DROP {_currentPocketItem.Value.itemType}");

                    if (_pocketItems[_index].count == 0)
                    {
                        print(_pocketItems[_index].count);
                        RemoveAt(_index);
                        _currentPocketItem = new NetworkVariable<Item>();
                    }
                }
            }
            else if (item.equipType == Item.EquipType.Hand)
            {
                hand.DestroyChild();
                ItemData.Instance.SpawnItem(item.itemType, item.count, transform.position, Quaternion.identity);
                _handItem = new NetworkVariable<Item>();
            }
        }
    }
}