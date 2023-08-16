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
        [SerializeField] private Item requiredItem;
        private NetworkVariable<bool> _started;

        private void Awake()
        {
            _started = new NetworkVariable<bool>();
        }

        public void Interact(Inventory inventory)
        {
            InteractServerRpc(inventory.gameObject);
        }

        [ServerRpc(RequireOwnership = false)]
        private void InteractServerRpc(NetworkObjectReference player)
        {
            if (_started.Value) return;
            if (player.TryGet(out NetworkObject playerNet))
            {
                var inventory = playerNet.GetComponent<Inventory>();

                if (inventory.HasItem(requiredItem, requiredItem.Count))
                {
                    inventory.Remove(requiredItem, requiredItem.Count);
                    _started.Value = true;
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