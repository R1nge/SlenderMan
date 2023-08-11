using TMPro;
using Unity.Netcode;
using UnityEngine;

namespace Characters.Human.UI
{
    public class HealthUI : NetworkBehaviour
    {
        [SerializeField] private TextMeshProUGUI healthText;
        private HumanHealthView _humanHealth;

        private void Awake()
        {
            _humanHealth = GetComponent<HumanHealthView>();
        }

        private void Start()
        {
            healthText.gameObject.SetActive(IsOwner);
            if (!IsOwner) return;
            _humanHealth.CurrentHealth.OnValueChanged += HealthChanged;
            HealthChanged(0, _humanHealth.CurrentHealth.Value);
        }

        private void HealthChanged(int _, int health)
        {
            healthText.text = health.ToString();
        }
    }
}