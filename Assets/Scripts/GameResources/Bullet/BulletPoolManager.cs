using System;
using System.Collections.Generic;
using CoreResources.Utils.Singletons;
using UnityEngine;

namespace GameResources.Bullet
{
    public class BulletPoolManager : MonoBehaviorSingleton<BulletPoolManager>
    {
        private const int _bulletCap = 250;
        private GameObject _primaryBullet;
        private GameObject _secondaryBullet;
        private Stack<GameObject> _primaryBulletPool;
        private Stack<GameObject> _secondaryBulletPool;
        private List<GameObject> _spawnedBullets;

        protected override void InitSingleton()
        {
            base.InitSingleton();
            _primaryBulletPool = new Stack<GameObject>();
            _secondaryBulletPool = new Stack<GameObject>();
            _spawnedBullets = new List<GameObject>();
            

            LoadBulletPools();
            InstantiateBulletPools();
        }

        private void LoadBulletPools()
        {
            _primaryBullet = AppHandler.AssetHandler.LoadAsset<GameObject>("Bullet");
            _secondaryBullet = AppHandler.AssetHandler.LoadAsset<GameObject>("WaveBullet");
        }

        private void InstantiateBulletPools()
        {
            for (int i = 0; i < _bulletCap; i++)
            {
                var primBul = Instantiate(_primaryBullet, transform.position, transform.rotation);
                var secBul = Instantiate(_secondaryBullet, transform.position, transform.rotation);
                primBul.SetActive(false);
                secBul.SetActive(false);
                _primaryBulletPool.Push(primBul);
                _secondaryBulletPool.Push(secBul);
            }
        }

        public GameObject SpawnPrimaryBullet(Vector3 position, Quaternion rotation, int damage = 1)
        {
            var GO = _primaryBulletPool.Pop();
            GO.SetActive(true);
            GO.transform.position = position;
            GO.transform.rotation = rotation;
            GO.GetComponent<BasicBullet>().SetDamage(damage);
            _spawnedBullets.Add(GO);
            return GO;
        }

        public GameObject SpawnSecondaryBullet(Vector3 position, Quaternion rotation, 
            Func<float, float> waveFunc, ModulationType modType = ModulationType.None, Func<float, float> modFunc = null, int damage = 1)
        {
            var GO = _secondaryBulletPool.Pop();
            GO.SetActive(true);
            GO.transform.position = position;
            GO.transform.rotation = rotation;
            WaveBullet WB = GO.GetComponent<WaveBullet>();
            WB.SetDamage(damage);
            WB.SetWaveSpecs(modType, waveFunc, modFunc);
            _spawnedBullets.Add(GO);
            return GO;
        }

        public void ReturnToPool(GameObject bullet)
        {
            bullet.SetActive(false);
            bullet.transform.position = transform.position;
            bullet.transform.rotation = transform.rotation;
            if (!_spawnedBullets.Remove(bullet))
            {
                throw new ArgumentOutOfRangeException($"Bullet does not exist in the spawned list");
            }
            WaveBullet WB = bullet.GetComponent<WaveBullet>();
            if (WB != null)
            {
                WB.ResetWaveSpecs();
                _secondaryBulletPool.Push(bullet);
            }
            else
            {
                _primaryBulletPool.Push(bullet);
            }
        }
    }
}