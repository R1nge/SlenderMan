using UnityEngine;

namespace Characters.Human
{
    public class Item
    {
        public ItemType Type { get; }
        public uint Count;

        public enum ItemType : int
        {
            Gasoline = 0,
            KeyHouseWithToilet = 1
        }

        public Item(ItemType type, uint count)
        {
            Type = type;
            Count = count;
        }
    }
}