using System.Collections;
using GameResources.Character;
using GameResources.Projectiles.Missile;
using UnityEngine;

namespace GameResources.Enemy.Boss
{
    public class BossMissile : RMissile
    {
        protected float localTime = 0f;
        protected float BulletFirePeriod;
        protected float BulletTranslationSpeed;

        private BossGun[] _missileGuns;

        public virtual void SetMissileStats(int damage, float missileSpeed, float trackingFactor, int bulletDamage, float bulletFirePeriod, float bulletTranslationSpeed)
        {
            Damage = damage;
            MissileSpeed = missileSpeed;
            TrackingFactor = trackingFactor;
            BulletFirePeriod = bulletFirePeriod;
            BulletTranslationSpeed = bulletTranslationSpeed;
            _missileGuns = GetComponentsInChildren<BossGun>();
            foreach (var gun in _missileGuns)
            {
                gun.bulletDamage = bulletDamage;
                gun.InitGun();
            }
        }

        public override void FireMissile()
        {
            localTime = 0f;
            base.FireMissile();
        }

        protected override IEnumerator GuidedMissileCoroutine()
        {
            while (true)
            {
                yield return null;
                localTime += Time.smoothDeltaTime;
                if (localTime >= MissileLifeTime)
                {
                    ReturnToPool();
                }
                rb.MovePosition(rb.position + transform.forward * MissileSpeed * Time.smoothDeltaTime);
                rb.MoveRotation(Quaternion.Slerp(
                    rb.rotation, 
                    Quaternion.LookRotation(Target.position - rb.position, -Vector3.forward), 
                    TrackingFactor));
                if (localTime >= BulletFirePeriod)
                {
                    foreach (var missileGun in _missileGuns)
                    {
                        missileGun.FireBullet();
                    }
                }
            }
        }
    }
}