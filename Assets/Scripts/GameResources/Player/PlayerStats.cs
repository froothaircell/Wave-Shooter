using GameResources.Character;
using GameResources.Events;
using UnityEngine;

namespace GameResources.Player
{
    public class PlayerStats : RCharacterStats
    {
        public int MaxShield = 100;
        private int _shield;


        public override void OnInit()
        {
            SetStats();
        }

        public override void OnUpdate()
        {
            
        }

        public override void OnDeInit()
        {
            
        }

        public override void SetStats()
        {
            _shield = MaxShield;
            base.SetStats();
        }

        public override void TakeDamage(int dmg)
        {
            if (_shield > dmg)
            {
                _shield -= dmg;
                return;
            }

            var healthDed = dmg;
            if (_shield > 0)
            {
                healthDed -= _shield;
                _shield = 0;
                Debug.Log("Player Shield Destroyed!");
                // Shield destroyed effect
            }
            base.TakeDamage(healthDed);
        }

        protected override void OnDeath()
        {
            Debug.Log($"Player Died!");
            REvent_PlayerDeath.Dispatch(transform);
        }
    }
}