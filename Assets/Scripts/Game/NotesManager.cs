using Unity.Netcode;
using UnityEngine;

namespace Game
{
    public class NotesManager : NetworkBehaviour
    {
        [SerializeField] private Note note;
        [SerializeField] private int notesAmount;
        [SerializeField] private Transform[] spawnPositions;
        private NetworkVariable<int> _collectedAmount;

        private void Awake()
        {
            _collectedAmount = new NetworkVariable<int>();
        }
        
        public void SpawnNotes()
        {
            int spawnAmount;
            if (notesAmount > spawnPositions.Length)
            {
                spawnAmount = spawnPositions.Length;
            }
            else
            {
                spawnAmount = notesAmount;
            }
            
            for (int i = 0; i < spawnAmount; i++)
            {
                var instance = Instantiate(note, spawnPositions[i].position, Quaternion.identity);
                instance.GetComponent<NetworkObject>().Spawn(true);
            }
        }
        
        public void Collect()
        {
            _collectedAmount.Value++;
            print($"Note collected; Only {notesAmount - _collectedAmount.Value} notes left");
        }
    }
}