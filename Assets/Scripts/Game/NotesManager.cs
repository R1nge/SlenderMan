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
        private int _spawnedAmount;
        private NetworkVariable<int> _collectedAmount;

        private void Awake()
        {
            _collectedAmount = new NetworkVariable<int>();
        }

        public void SpawnNotes()
        {
            if (notesAmount > spawnPositions.Length)
            {
                _spawnedAmount = spawnPositions.Length;
            }
            else
            {
                _spawnedAmount = notesAmount;
            }

            for (int i = 0; i < _spawnedAmount; i++)
            {
                var instance = Instantiate(note, spawnPositions[i].position, Quaternion.identity);
                instance.GetComponent<NetworkObject>().Spawn(true);
            }
        }

        public void Collect()
        {
            _collectedAmount.Value++;
            print($"Note collected; Only {_spawnedAmount - _collectedAmount.Value} notes left");
            if (_collectedAmount.Value == _spawnedAmount)
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