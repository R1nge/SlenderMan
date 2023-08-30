﻿using System;
using Unity.Netcode;

namespace Characters.Human
{
    [Serializable]
    public struct Item : INetworkSerializable, IEquatable<Item>
    {
        public EquipType equipType;
        public ItemType itemType;
        public uint count;
        public bool spawn;

        public enum ItemType
        {
            None,
            Gasoline = 1,
            Battery = 2,
            KeyHouseWithToilet = 3,
            Shotgun = 4
        }

        public enum EquipType
        {
            Pocket = 0,
            Hand = 1
        }

        public Item(EquipType equipType, ItemType itemType, uint count, bool spawn)
        {
            this.equipType = equipType;
            this.itemType = itemType;
            this.count = count;
            this.spawn = spawn;
        }

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref equipType);
            serializer.SerializeValue(ref itemType);
            serializer.SerializeValue(ref count);
            serializer.SerializeValue(ref spawn);
        }

        public bool Equals(Item other)
        {
            return equipType == other.equipType && itemType == other.itemType && count == other.count && spawn == other.spawn;
        }

        public override bool Equals(object obj)
        {
            return obj is Item other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine((int)equipType, (int)itemType, count, spawn);
        }
    }
}