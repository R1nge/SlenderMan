using System;
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
        [SerializeField] private AudioSource sound;
        [SerializeField] private AudioClip[] clips;

        private void Awake()
        {
            generator.OnInteracted += Interacted;
            sound.clip = clips[0];
        }

        private void Interacted()
        {
            InteractedServerRpc();
        }

        [ServerRpc(RequireOwnership = false)]
        private void InteractedServerRpc()
        {
            wall.SetActive(false);
            sound.Play();
            InteractedClientRpc();
            StartCoroutine(WaitForTheEnd(sound));
        }

        private IEnumerator WaitForTheEnd(AudioSource source)
        {
            yield return new WaitForSeconds(source.clip.length);
            source.clip = clips[1];
            sound.loop = true;
            source.Play();
        }

        [ClientRpc]
        private void InteractedClientRpc()
        {
            if (IsServer) return;
            sound.Play();
            wall.SetActive(false);
            StartCoroutine(WaitForTheEnd(sound));
        }
    }
}