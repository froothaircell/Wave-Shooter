using GameResources.Events;

namespace GameResources.Enemy.Boss
{
    public class BossStats : EnemyStats
    {
        public override void TakeDamage(int dmg)
        {
            base.TakeDamage(dmg);
            if (_health <= maxHealth / 2)
            {
                OnHalfHealth();
            }
        }

        private void OnHalfHealth()
        {
            REvent_BossHalfHealth.Dispatch();
        }
    }
}