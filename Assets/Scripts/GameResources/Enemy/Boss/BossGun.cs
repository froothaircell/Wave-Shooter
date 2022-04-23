using BulletFury;
using GameResources.Gun;
using UnityEngine;

namespace GameResources.Enemy.Boss
{
    [RequireComponent(typeof(BulletManager))]
    public class BossGun : RGun
    {
        private BulletManager _gunBulletSpawner;
        
        public override void InitGun()
        {
            _firePoint = transform;
            _gunBulletSpawner = GetComponent<BulletManager>();
        }

        public override void FireBullet()
        {
            _gunBulletSpawner.Spawn(_firePoint.position, _firePoint.up);
        }
    }
}