using System;
using GameResources.Gun;
using UnityEngine;
using UnityEngine.Serialization;

namespace GameResources.Character
{
    public abstract class RCharacterStats : MonoBehaviour, ICharacterStats, ICharacterComponent
    {
        public int maxHealth = 200;
        protected int _health;

        public abstract void OnInit();

        public abstract void OnUpdate();

        public abstract void OnDeInit();

        public virtual void SetStats()
        {
            _health = maxHealth;
        }
        
        public virtual void TakeDamage(int dmg)
        {
            _health -= dmg;
            if (_health <= 0)
            {
                OnDeath();
            }
        }

        // protected abstract void OnCollisionEnter(Collision collision);

        protected abstract void OnDeath();
    }
}