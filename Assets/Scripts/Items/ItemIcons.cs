using System;
using System.Collections.Generic;
using Characters.Human;
using Sirenix.OdinInspector;
using Unity.Netcode;
using UnityEngine;

namespace Items
{
    public class ItemIcons : SerializedMonoBehaviour
    {
        [SerializeField] private Dictionary<Item.ItemType, Sprite> _items = new();
        public static ItemIcons Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null)
            {
                throw new Exception("Multiple Item Spawners defined!");
            }

            DontDestroyOnLoad(gameObject);
            Instance = this;
        }

        public Sprite GetIcon(Item.ItemType type) => _items[type];
    }
}