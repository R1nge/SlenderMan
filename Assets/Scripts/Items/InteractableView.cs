using System;
using Characters.Human;
using Characters.Human.Interact;
using Unity.Netcode;
using UnityEngine;

namespace Items
{
    public class InteractableView : NetworkBehaviour, IInteractable
    {
        public event Action OnInteracted;
        [SerializeField] private Item requiredItem;
        private NetworkVariable<bool> _interacted;

        public bool Interacted => _interacted.Value;

        private void Awake()
        {
            _interacted = new NetworkVariable<bool>();
        }

        public void Interact(Inventory inventory)
        {
            InteractServerRpc(inventory.gameObject);
        }

        [ServerRpc(RequireOwnership = false)]
        private void InteractServerRpc(NetworkObjectReference player)
        {
            if (_interacted.Value) return;
            if (player.TryGet(out NetworkObject playerNet))
            {
                var inventory = playerNet.GetComponent<Inventory>();

                if (inventory.HasItem(requiredItem, requiredItem.count))
                {
                    inventory.Remove(requiredItem, requiredItem.count);
                    _interacted.Value = true;
                    OnInteracted?.Invoke();
                    print("INTERACT SERVER RPC");
                    InteractClientRpc();
                }
            }
        }

        [ClientRpc]
        private void InteractClientRpc()
        {
            if (IsServer) return;
            OnInteracted?.Invoke();
            print("INTERACT CLIENT RPC");
        }
    }
}