using BulletFury;
using GameResources.Gun;
using UnityEngine;

namespace GameResources.Enemy.Boss
{
    [RequireComponent(typeof(BulletManager))]
    public class BossGun : RGun
    {
        protected BulletManager _gunBulletSpawner;
        
        public override void InitGun()
        {
            _firePoint = transform;
            _gunBulletSpawner = GetComponent<BulletManager>();
            _gunBulletSpawner.GetBulletSettings().SetDamage(bulletDamage);
        }

        public override void FireBullet()
        {
            _gunBulletSpawner.Spawn(_firePoint.position, _firePoint.up);
        }
    }
}