using System;
using Items;
using Unity.Netcode;
using UnityEngine;

namespace Characters.Human.Interact
{
    public class OnInteractGenerator : NetworkBehaviour
    {
        [SerializeField] private GameObject wall;
        [SerializeField] private InteractableView generator;

        private void Awake()
        {
            generator.OnInteracted += Interacted;
        }

        private void Interacted()
        {
            InteractedServerRpc();
        }

        [ServerRpc(RequireOwnership = false)]
        private void InteractedServerRpc()
        {
            wall.SetActive(false);
            InteractedClientRpc();
        }

        [ClientRpc]
        private void InteractedClientRpc()
        {
            if (IsServer) return;
            wall.SetActive(false);
        }
    }
}