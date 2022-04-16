using UnityEngine;

namespace GameResources.Player
{
    // Change to non monobehavior after you have a pool manager
    public class Gun : MonoBehaviour, IGun
    {
        public float bulletSpeed = 10f;
        public GameObject bullet;
        public GameObject specBullet;
        private Transform _firePoint;
        
        public void OnInit()
        {
            _firePoint = transform.GetChild(0);
            GunController controller = GetComponentInParent<GunController>();
            controller.OnFire += FireBullet;
            controller.OnFireSpec += FireSpecialBullet;
        }

        public void OnDeInit()
        {
            
        }

        private void FireBullet()
        {
            Debug.Log($"Fired bullet with the current direction: {_firePoint.up * bulletSpeed}");
            var projectile = Instantiate(bullet, _firePoint.position, _firePoint.rotation);
            projectile.GetComponent<Rigidbody>().AddForce(_firePoint.up * bulletSpeed, ForceMode.VelocityChange);
        }

        private void FireSpecialBullet()
        {
            Debug.Log($"Fired special bullet with the current direction: {_firePoint.up * bulletSpeed}");
            var projectile = Instantiate(specBullet, _firePoint.position, _firePoint.rotation);
            projectile.GetComponent<Rigidbody>().AddForce(_firePoint.up * bulletSpeed, ForceMode.VelocityChange);
        }
    }
}
