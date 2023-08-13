using Characters.Human;
using Characters.Human.Interact;
using Unity.Netcode;

namespace Game
{
    public class Note : NetworkBehaviour, IIntractable
    {
        private NotesManager _notesManager;

        private void Awake()
        {
            _notesManager = FindObjectOfType<NotesManager>();
        }

        public void Interact(Inventory inventory)
        {
            if (!IsServer) return;
            _notesManager.Collect();
            NetworkObject.Despawn(true);
        }
    }
}