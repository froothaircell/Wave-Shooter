using GameResources.Player;
using UnityEngine;

namespace GameResources.Gun
{
    public abstract class RGun : MonoBehaviour, IGun
    {
        public int bulletDamage = 1;
        
        public float bulletSpeed = 10f;
        public GameObject primaryBullet;
        protected Transform _firePoint;
        
        public float PrimaryFireRate { get; private set; }
        
        public virtual void OnInit()
        {
            _firePoint = transform.GetChild(0);
            GunController controller = GetComponentInParent<GunController>();
            controller.OnFire += FireBullet;
        }

        public virtual void OnDeInit()
        {
            
        }

        protected virtual void FireBullet()
        {
            var projectile =
                AppHandler.BulletManager.SpawnPrimaryBullet(_firePoint.position, _firePoint.rotation, bulletDamage);
            projectile.GetComponent<Rigidbody>().AddForce(_firePoint.up * bulletSpeed, ForceMode.VelocityChange);
        }
    }
}