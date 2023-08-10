using System;
using Unity.Netcode;
using UnityEngine;

namespace Game
{
    public class NotesManager : NetworkBehaviour
    {
        [SerializeField] private Note note;
        [SerializeField] private int notesAmount;
        [SerializeField] private Transform[] spawnPositions;
        public static NotesManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null)
            {
                throw new Exception("Multiple NotesManagers defined!");
            }

            DontDestroyOnLoad(gameObject);
            Instance = this;
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
    }
}