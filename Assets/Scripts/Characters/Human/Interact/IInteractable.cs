namespace Characters.Human.Interact
{
    public interface IInteractable
    {
        void Interact(Inventory inventory);
        bool Interacted { get; }
    }
}