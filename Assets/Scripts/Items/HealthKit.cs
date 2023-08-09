using Characters.Human;

namespace Items
{
    public class HealthKit
    {
        private int _heal;

        public void SetHealAmount(int heal)
        {
            _heal = heal;
        }

        public void Heal(HumanHealthView humanHealth)
        {
            humanHealth.IncreaseServerRpc(_heal);
        }
    }
}