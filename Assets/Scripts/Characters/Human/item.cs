using System;
using Unity.Netcode;

namespace Characters.Human
{
    [Serializable]
    public struct Item : INetworkSerializable,IEquatable<Item>
    {
        public ItemType Type;
        public uint Count;

        public enum ItemType
        {
            Gasoline = 0,
            KeyHouseWithToilet = 1
        }

        public Item(ItemType type, uint count)
        {
            Type = type;
            Count = count;
        }

        public bool Equals(Item other)
        {
            return Type == other.Type && Count == other.Count;
        }

        public override bool Equals(object obj)
        {
            return obj is Item other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine((int)Type, Count);
        }

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref Type);
            serializer.SerializeValue(ref Count);
        }
    }
}