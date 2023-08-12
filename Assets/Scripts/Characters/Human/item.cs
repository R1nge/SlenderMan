namespace Characters.Human
{
    public class Item
    {
        public ItemType Type { get; }

        public enum ItemType
        {
            Gasoline,
            KeyHouseWithToilet
        }

        public Item(ItemType type)
        {
            Type = type;
        }
    }
}