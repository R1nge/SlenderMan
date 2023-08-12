using Unity.Netcode;
using UnityEngine;

namespace Characters.Human.Interact
{
    public class Door : MonoBehaviour, IIntractable
    {
        [SerializeField] private Animator animator;
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
            animator.SetBool(Open, _open);
        }
    }
}