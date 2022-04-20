using GameResources.Gun;

namespace GameResources.Enemy
{
    public class MookGun : RGun
    {
        public override void OnInit()
        {
            _firePoint = transform;
        }

        public override void FireBullet()
        {
            AppHandler.BulletManager.SpawnEnemyBullet(transform.position, _firePoint.up);
        }
    }
}