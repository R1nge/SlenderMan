using Unity.Netcode;
using UnityEngine;

namespace Game
{
    public class Note : NetworkBehaviour
    {
        private NotesManager _notesManager;

        private void Awake()
        {
            _notesManager = FindObjectOfType<NotesManager>();
        }

        private void OnTriggerEnter(Collider other)
        {
            Collect();
        }
        
        private void Collect()
        {
            if (!IsServer) return;
            _notesManager.Collect();
            NetworkObject.Despawn(true);
        }
    }
}