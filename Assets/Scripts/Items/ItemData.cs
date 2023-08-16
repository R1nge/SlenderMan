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

        public void Spawn(Item.ItemType item, uint amount, Vector3 position)
        {
            for (int i = 0; i < amount; i++)
            {
                var inst = Instantiate(_items[item].Prefab, position, Quaternion.identity);
                inst.GetComponent<NetworkObject>().Spawn();
            }
        }
    }
}