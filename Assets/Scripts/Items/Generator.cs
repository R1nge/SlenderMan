using System;
using Characters.Human;
using Characters.Human.Interact;
using Unity.Netcode;
using UnityEngine;

namespace Items
{
    public class Generator : NetworkBehaviour, IIntractable
    {
        public event Action OnGeneratorStarted;
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
            OnGeneratorStarted?.Invoke();
            print("GENERATOR: INTERACT SERVER RPC");
            InteractClientRpc();
        }

        [ClientRpc]
        private void InteractClientRpc()
        {
            if (IsServer) return;
            OnGeneratorStarted?.Invoke();
            print("GENERATOR: INTERACT CLIENT RPC");
        }
    }
}