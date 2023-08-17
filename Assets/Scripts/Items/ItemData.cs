using System;
using System.Collections.Generic;
using Characters.Human;
using Sirenix.OdinInspector;
using Unity.Netcode;
using UnityEngine;

namespace Items
{
    public class ItemData : SerializedMonoBehaviour
    {
        [SerializeField] private Dictionary<Item.ItemType, ItemDataSo> _items = new();
        [SerializeField] private Dictionary<Item.ItemType, ItemDataSo> _models = new();
        public static ItemData Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null)
            {
                throw new Exception("Multiple Item Data defined!");
            }

            DontDestroyOnLoad(gameObject);
            Instance = this;
        }

        public Sprite GetIcon(Item.ItemType type) => _items[type].Icon;

        public NetworkObject SpawnItem(Item.ItemType item, uint amount, Vector3 position, Quaternion rotation,
            Transform parent = null)
        {
            var inst = Instantiate(_items[item].Prefab, position, rotation, parent);
            var net = inst.GetComponent<NetworkObject>();
            net.Spawn();
            net.transform.parent = parent;
            net.GetComponent<ItemView>().SetCount(amount);
            return net;
        }

        public GameObject SpawnModel(Item.ItemType item, Vector3 position, Quaternion rotation, Transform parent = null)
        {
            var inst = Instantiate(_models[item].Prefab, position, rotation, parent);
            inst.transform.parent = parent;
            return inst;
        }
    }
}