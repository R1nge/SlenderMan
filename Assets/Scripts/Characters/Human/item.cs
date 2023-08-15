using UnityEngine;

namespace Characters.Human
{
    public class Item
    {
        public ItemType Type { get; }
        public uint Count;

        public enum ItemType
        {
            Gasoline,
            KeyHouseWithToilet
        }

        public Item(ItemType type, uint count)
        {
            Type = type;
            Count = count;
        }
    }
}