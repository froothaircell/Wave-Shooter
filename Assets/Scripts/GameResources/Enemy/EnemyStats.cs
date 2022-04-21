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
            Debug.Log("Got into ontriggerenter");
            if (other.CompareTag("PlayerBullet"))
            {
                Debug.Log("Taking damage");
                TakeDamage(other.GetComponentInParent<RBullet>().Damage);
            }
        }
    }
}