using Characters.Human;
using Unity.Netcode;
using UnityEngine;

namespace Items
{
    public class HealthKitView : NetworkBehaviour
    {
        [SerializeField] private int heal;
        private HealthKit _healthKit;

        private void Awake()
        {
            _healthKit = new HealthKit();
            _healthKit.SetHealAmount(heal);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out HumanHealthView humanHealth))
            {
                var currentHealth = humanHealth.CurrentHealth.Value;
                var maxHealth = humanHealth.MaxHealth.Value;
                var dead = humanHealth.IsDead.Value;
                if (!dead && currentHealth < maxHealth)
                {
                    _healthKit.Heal(humanHealth);
                    NetworkObject.Despawn();
                }
            }
        }
    }
}