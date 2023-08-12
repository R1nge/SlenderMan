using System.Collections.Generic;
using UnityEngine;

namespace Characters.Human
{
    public class Inventory : MonoBehaviour
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
    }
}