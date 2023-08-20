using Characters.Human;
using Characters.Human.Interact;
using Unity.Netcode;

namespace Game
{
    public class Note : NetworkBehaviour, IIntractable
    {
        public bool Interacted { get; }
        private NotesManager _notesManager;

        private void Awake()
        {
            _notesManager = FindObjectOfType<NotesManager>();
        }

        public void Interact(Inventory inventory)
        {
            InteractServerRpc();
        }

        [ServerRpc(RequireOwnership = false)]
        private void InteractServerRpc()
        {
            _notesManager.Collect();
            NetworkObject.Despawn(true);
        }
    }
}