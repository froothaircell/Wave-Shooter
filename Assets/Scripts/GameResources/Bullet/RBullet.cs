using System;
using UnityEngine;

namespace GameResources.Bullet
{
    public abstract class RBullet : MonoBehaviour, IBullet
    {
        public int Damage { get; private set; }

        public virtual void OnInit()
        {
            
        }

        public virtual void OnUpdate()
        {
            
        }

        public virtual void OnDeInit()
        {
            
        }

        public void SetDamage(int val)
        {
            Damage = val;
        }

        private void ReturnToPool()
        {
            AppHandler.BulletManager.ReturnToPool(gameObject);
        }
    }
}