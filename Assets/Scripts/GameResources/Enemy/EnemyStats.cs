using System;
using GameResources.Bullet;
using GameResources.Character;
using GameResources.Events;
using UnityEngine;

namespace GameResources.Enemy
{
    public class EnemyStats: RCharacterStats
    {
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

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("PlayerBullet"))
            {
                var BulletCon = other.GetComponentInParent<RBullet>();
                TakeDamage(BulletCon.Damage);
                BulletCon.ReturnToPool();
            }
        }
    }
}