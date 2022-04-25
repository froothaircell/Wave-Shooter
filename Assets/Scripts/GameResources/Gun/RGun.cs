using UnityEngine;

namespace GameResources.Gun
{
    public abstract class RGun : MonoBehaviour
    {
        public int bulletDamage = 1;
        public float bulletTranslationSpeed = 10f;
        
        protected Transform _firePoint;
        
        public virtual void InitGun()
        {
            _firePoint = transform.GetChild(0);
        }

        public virtual void FireBullet()
        {
            var projectile =
                AppHandler.BulletManager.SpawnPrimaryBullet(_firePoint.position, _firePoint.rotation, bulletDamage, bulletTranslationSpeed);
        }
    }
}