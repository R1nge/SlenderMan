using System;
using Characters.Human;
using Characters.Human.Interact;
using Unity.Netcode;
using UnityEngine;

namespace Items
{
    public class InteractableView : NetworkBehaviour, IIntractable
    {
        public event Action OnInteracted;
        [SerializeField] private Item.ItemType requiredItem;
        private NetworkVariable<bool> _started;

        private void Awake()
        {
            _started = new NetworkVariable<bool>();
        }

        public void Interact(Inventory inventory)
        {
            if (inventory.HasItem(requiredItem))
            {
                inventory.Remove(requiredItem);
                InteractServerRpc();
            }
        }

        [ServerRpc(RequireOwnership = false)]
        private void InteractServerRpc()
        {
            if (_started.Value) return;
            _started.Value = true;
            OnInteracted?.Invoke();
            print("INTERACT SERVER RPC");
            InteractClientRpc();
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