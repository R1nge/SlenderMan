using System.Collections;
using Items;
using Unity.Netcode;
using UnityEngine;

namespace Characters.Human.Interact
{
    public class OnInteractGenerator : NetworkBehaviour
    {
        [SerializeField] private GameObject wall;
        [SerializeField] private InteractableView generator;
        [SerializeField] private AudioSource[] sources;

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
            InteractedClientRpc();
        }

        [ClientRpc]
        private void InteractedClientRpc()
        {
            sources[0].Play();
            wall.SetActive(false);
            StartCoroutine(WaitForTheEnd());
        }

        private IEnumerator WaitForTheEnd()
        {
            yield return new WaitForSeconds(sources[0].clip.length);
            sources[1].Play();
        }
    }
}