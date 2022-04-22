﻿namespace GameResources.Enemy
{
    public class KamikaziMookStats : EnemyStats
    {
        private MookExplosionController _explosionController;
        
        public override void OnInit()
        {
            base.OnInit();
            _explosionController = GetComponentInChildren<MookExplosionController>();
        }

        protected override void OnDeath()
        {
            _explosionController.Explode();
            base.OnDeath();
        }
    }
}