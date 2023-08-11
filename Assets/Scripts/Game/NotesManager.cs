using System;
using Unity.Netcode;
using UnityEngine;

namespace Game
{
    public class NotesManager : NetworkBehaviour
    {
        public event Action OnAllNotesCollected;
        [SerializeField] private Note note;
        [SerializeField] private int notesAmount;
        [SerializeField] private Transform[] spawnPositions;
        private NetworkVariable<int> _spawnedAmount;
        private NetworkVariable<int> _collectedAmount;

        public NetworkVariable<int> CollectedAmount() => _collectedAmount;

        public int TotalAmount() => _spawnedAmount.Value;

        private void Awake()
        {
            _spawnedAmount = new NetworkVariable<int>();
            _collectedAmount = new NetworkVariable<int>();
        }

        public void SpawnNotes()
        {
            if (notesAmount > spawnPositions.Length)
            {
                _spawnedAmount.Value = spawnPositions.Length;
            }
            else
            {
                _spawnedAmount.Value = notesAmount;
            }

            for (int i = 0; i < _spawnedAmount.Value; i++)
            {
                var instance = Instantiate(note, spawnPositions[i].position, Quaternion.identity);
                instance.GetComponent<NetworkObject>().Spawn(true);
            }
        }

        public void Collect()
        {
            _collectedAmount.Value++;
            if (_collectedAmount.Value == _spawnedAmount.Value)
            {
                OnAllNotesCollected?.Invoke();
                OnAllNotesCollectedClientRpc();
            }
        }

        [ClientRpc]
        private void OnAllNotesCollectedClientRpc()
        {
            if(IsServer) return;
            OnAllNotesCollected?.Invoke();
        }
    }
}