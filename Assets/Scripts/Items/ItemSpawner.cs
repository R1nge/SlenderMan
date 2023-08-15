using System;
using System.Collections.Generic;
using Characters.Human;
using Sirenix.OdinInspector;
using Unity.Netcode;
using UnityEngine;

namespace Items
{
    public class ItemSpawner : SerializedMonoBehaviour
    {
        [SerializeField] private Dictionary<Item.ItemType, GameObject> _items = new();
        public static ItemSpawner Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null)
            {
                throw new Exception("Multiple Item Spawners defined!");
            }

            DontDestroyOnLoad(gameObject);
            Instance = this;
        }

        public void Spawn(Item.ItemType item, uint amount, Vector3 position)
        {
            for (int i = 0; i < amount; i++)
            {
                var inst = Instantiate(_items[item], position, Quaternion.identity);
                inst.GetComponent<NetworkObject>().Spawn();
            }
        }
    }
}