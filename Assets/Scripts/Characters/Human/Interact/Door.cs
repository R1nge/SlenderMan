using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

namespace Characters.Human.Interact
{
    public class Door : NetworkBehaviour, IIntractable
    {
        public bool Interacted { get; }
        [SerializeField] private NetworkAnimator animator;
        private bool _open;
        private static readonly int Open = Animator.StringToHash("Open");


        public void Interact(Inventory inventory)
        {
            print("Interacted");
            InteractServerRpc();
        }

        [ServerRpc(RequireOwnership = false)]
        private void InteractServerRpc()
        {
            _open = !_open;
            animator.Animator.SetBool(Open, _open);
        }
    }
}