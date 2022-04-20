using System;
using CoreResources.Utils;
using GameResources.Bullet;
using GameResources.Gun;
using UnityEngine;

namespace GameResources.Player
{
    // Change to non monobehavior after you have a pool manager
    public class PlayerGun : RGun
    {
        public ModulationType _modType;
        public WaveType _waveType;
        public WaveType _modWaveType;
        
        public override void InitGun()
        {
            _modType = ModulationType.None;
            _waveType = WaveType.None;
            _modWaveType = WaveType.None;
            _firePoint = transform.GetChild(0);
        }

        public void FireSpecialBullet()
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
                default:    // Change this later
                    waveFunc = WaveGenerator.Sin;
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
                waveFunc, _modType, modFunc, bulletDamage, bulletTranslationSpeed);
            // var projectile = Instantiate(specBullet, _firePoint.position, _firePoint.rotation);
            // projectile.GetComponent<Rigidbody>().AddForce(_firePoint.up * bulletSpeed, ForceMode.VelocityChange);
        }
    }
}
