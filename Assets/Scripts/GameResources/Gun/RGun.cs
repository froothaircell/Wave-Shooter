﻿using GameResources.Character;
using GameResources.Player;
using UnityEngine;

namespace GameResources.Gun
{
    public abstract class RGun : MonoBehaviour, ICharacterComponent
    {
        public int bulletDamage = 1;
        public float bulletTranslationSpeed = 10f;
        
        protected Transform _firePoint;
        
        public float PrimaryFireRate { get; private set; }
        
        public virtual void OnInit()
        {
            _firePoint = transform.GetChild(0);
        }

        public virtual void OnUpdate()
        {
            
        }

        public virtual void OnDeInit()
        {
            
        }

        public virtual void FireBullet()
        {
            var projectile =
                AppHandler.BulletManager.SpawnPrimaryBullet(_firePoint.position, _firePoint.rotation, bulletDamage, bulletTranslationSpeed);
        }
    }
}