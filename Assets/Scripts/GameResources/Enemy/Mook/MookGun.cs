using GameResources.Gun;

namespace GameResources.Enemy.Mook
{
    public class MookGun : RGun
    {
        public override void InitGun()
        {
            _firePoint = transform;
        }

        public override void FireBullet()
        {
            AppHandler.BulletManager.SpawnEnemyBullet(transform.position, _firePoint.up);
        }
    }
}