using Unity.Netcode;

namespace Characters.Human.Pickup
{
    public abstract class Pickupable : NetworkBehaviour
    {
        private bool _hasOwner;

        public void SetOwner() => _hasOwner = true;

        public void RemoveOwner() => _hasOwner = false;

        public bool HasOwner => _hasOwner;
        public abstract void Pickup(Inventory inventory);
    }
}