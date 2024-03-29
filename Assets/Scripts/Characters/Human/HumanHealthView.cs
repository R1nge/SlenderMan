﻿using Game;
using Unity.Netcode;
using UnityEngine;

namespace Characters.Human
{
    public class HumanHealthView : NetworkBehaviour
    {
        [SerializeField] private NetworkVariable<int> maxHealth, currentHealth;
        private NetworkVariable<bool> _isDead;
        private PlayerSpawner _playerSpawner;

        public NetworkVariable<int> CurrentHealth => currentHealth;
        public NetworkVariable<int> MaxHealth => maxHealth;
        public NetworkVariable<bool> IsDead => _isDead;

        private void Awake()
        {
            _isDead = new NetworkVariable<bool>();
            _playerSpawner = FindObjectOfType<PlayerSpawner>();
        }

        [ServerRpc(RequireOwnership = false)]
        public void IncreaseServerRpc(int amount)
        {
            if (_isDead.Value)
            {
                Debug.LogError("Trying to increase health on the dead player", this);
                return;
            }

            if (amount <= 0)
            {
                Debug.LogError($"Trying to increase by {amount}");
                return;
            }

            currentHealth.Value = Mathf.Clamp(currentHealth.Value + amount, 0, maxHealth.Value);
        }

        [ServerRpc(RequireOwnership = false)]
        public void ReduceServerRpc(int amount, ulong ownerId)
        {
            if (_isDead.Value)
            {
                Debug.LogError("Trying to reduce health on the dead player", this);
                return;
            }

            if (amount <= 0)
            {
                Debug.LogError($"Trying to reduce by {amount}");
                return;
            }

            currentHealth.Value = Mathf.Clamp(currentHealth.Value - amount, 0, maxHealth.Value);

            if (currentHealth.Value == 0)
            {
                Die(ownerId);
            }
        }

        private void Die(ulong id)
        {
            _isDead.Value = true;
            Debug.LogError("Player has died", this);
            _playerSpawner.DeSpawnServerRpc(NetworkObject, id);
        }
    }
}