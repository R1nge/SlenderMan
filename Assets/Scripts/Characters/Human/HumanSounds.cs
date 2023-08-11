using Unity.Netcode;
using UnityEngine;

namespace Characters.Human
{
    public class HumanSounds : NetworkBehaviour
    {
        [SerializeField] private AudioSource deathSoundPrefab;
        private HumanHealthView _humanHealth;

        private void Start()
        {
            if (!IsOwner) return;
            _humanHealth = GetComponent<HumanHealthView>();
            _humanHealth.IsDead.OnValueChanged += OnDeath;
        }

        private void OnDeath(bool _, bool isDead)
        {
            SpawnDeathSoundServerRpc();
        }

        [ServerRpc]
        private void SpawnDeathSoundServerRpc()
        {
            var sound = Instantiate(deathSoundPrefab, transform.position, Quaternion.identity);
            sound.GetComponent<NetworkObject>().Spawn(true);
        }
    }
}