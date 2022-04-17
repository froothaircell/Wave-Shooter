using System;
using CoreResources.Utils;
using GameResources.Bullet;
using GameResources.Gun;
using UnityEngine;
using UnityEngine.Serialization;

namespace GameResources.Player
{
    // Change to non monobehavior after you have a pool manager
    public class PlayerGun : RGun
    {
        private ModulationType _modType;
        private WaveType _waveType;
        private WaveType _modWaveType;
        
        public override void OnInit()
        {
            _modType = ModulationType.None;
            _waveType = WaveType.None;
            _modWaveType = WaveType.None;
            _firePoint = transform.GetChild(0);
            GunController controller = GetComponentInParent<GunController>();
            controller.OnFire += FireBullet;
            controller.OnFireSpec += FireSpecialBullet;
        }

        public override void OnDeInit()
        {
            
        }

        private void FireSpecialBullet()
        {
            Func<float, float> waveFunc = null, modFunc = null;
            switch (_waveType)
            {
                case WaveType.Sine:
                    waveFunc = WaveGenerator.Sin;
                    break;
                case WaveType.Tri:
                    waveFunc = WaveGenerator.Tri;
                    break;
                case WaveType.Sqr:
                    waveFunc = WaveGenerator.Sqr;
                    break;
                case WaveType.Saw:
                    waveFunc = WaveGenerator.Saw;
                    break;
            }

            switch (_modWaveType)
            {
                case WaveType.Sine:
                    modFunc = WaveGenerator.Sin;
                    break;
                case WaveType.Tri:
                    modFunc = WaveGenerator.Tri;
                    break;
                case WaveType.Sqr:
                    modFunc = WaveGenerator.Sqr;
                    break;
                case WaveType.Saw:
                    modFunc = WaveGenerator.Saw;
                    break;
            }

            var projectile = AppHandler.BulletManager.SpawnSecondaryBullet(_firePoint.position, _firePoint.rotation,
                waveFunc, _modType, modFunc, bulletDamage);
            // var projectile = Instantiate(specBullet, _firePoint.position, _firePoint.rotation);
            projectile.GetComponent<Rigidbody>().AddForce(_firePoint.up * bulletSpeed, ForceMode.VelocityChange);
        }
    }
}
