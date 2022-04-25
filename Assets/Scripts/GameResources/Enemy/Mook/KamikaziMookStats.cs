using System;
using UnityEngine;

namespace GameResources.Enemy.Mook
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

        private void OnCollisionEnter(Collision collision)
        {
            if (!collision.collider.CompareTag("PlayerBullet"))
            {
                TakeDamage(_health);
            }
        }
    }
}