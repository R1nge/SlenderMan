namespace Characters.Human.Interact
{
    public interface IIntractable
    {
        void Interact(Inventory inventory);
        bool Interacted { get; }
    }
}