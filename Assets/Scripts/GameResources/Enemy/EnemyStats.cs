﻿using GameResources.Character;
using GameResources.Events;

namespace GameResources.Enemy
{
    public class EnemyStats: RCharacterStats
    {
        public int maxHealth = 20;
        
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

        protected override void OnDeath()
        {
            REvent_MookDeath.Dispatch(transform);
        }
    }
}