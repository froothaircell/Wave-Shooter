using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using CoreResources.Utils.Singletons;
using UnityEngine;

namespace GameResources.Bullet
{
    public class BulletPoolManager : MonoBehaviorSingleton<BulletPoolManager>
    {
        private const int _bulletCap = 250;
        private const int _maxOpsPerFixedUpdate = 50; // This ensures batching during a coroutine
        private GameObject _primaryBullet;
        private GameObject _secondaryBullet;
        private Stack<GameObject> _primaryBulletPool;
        private Stack<GameObject> _secondaryBulletPool;
        private GameObject[] _spawnedBullets;
        private Dictionary<int, int> _availableIndices;
        private int _spawnCount = 0;
        private Coroutine _bulletUpdates;


        protected override void InitSingleton()
        {
            base.InitSingleton();
            _primaryBulletPool = new Stack<GameObject>();
            _secondaryBulletPool = new Stack<GameObject>();
            _spawnedBullets = new GameObject[500];
            _availableIndices = new Dictionary<int, int>();
            _spawnCount = 0;

            LoadBulletPools();
            InstantiateBulletPools();
        }

        private void LoadBulletPools()
        {
            _primaryBullet = AppHandler.AssetManager.LoadAsset<GameObject>("BasicBullet");
            _secondaryBullet = AppHandler.AssetManager.LoadAsset<GameObject>("WaveBullet");
        }

        private void InstantiateBulletPools()
        {
            for (int i = 0; i < _bulletCap; i++)
            {
                var primBul = Instantiate(_primaryBullet, transform.position, transform.rotation, transform);
                var secBul = Instantiate(_secondaryBullet, transform.position, transform.rotation, transform);
                primBul.SetActive(false);
                secBul.SetActive(false);
                _primaryBulletPool.Push(primBul);
                _secondaryBulletPool.Push(secBul);
                _availableIndices.Add(i, i);
            }

            for (int i = _bulletCap; i < (2 * _bulletCap); i++)
            {
                _availableIndices.Add(i, i);
            }
        }

        public GameObject SpawnPrimaryBullet(Vector3 position, Quaternion rotation, int damage = 1, 
            float bulletTranslationSpeed = 10f)
        {
            GameObject GO;
            if (!_primaryBulletPool.TryPop(out GO))
            {
                throw new ObjectNotFoundException($"Bullet Pool is empty!");
            }
            // var GO = _primaryBulletPool.Pop();
            GO.SetActive(true);
            GO.transform.position = position;
            GO.transform.rotation = rotation;
            BasicBullet BB = GO.GetComponent<BasicBullet>();
            var Index = _availableIndices.First().Value;
            BB.SetSpawnedBulletSpecs(damage, bulletTranslationSpeed, Index);
            BB.OnSpawn();
            _spawnedBullets[Index] = GO;
            _availableIndices.Remove(Index);
            _spawnCount++;
            return GO;
        }

        public GameObject SpawnSecondaryBullet(Vector3 position, Quaternion rotation, 
            Func<float, float> waveFunc, ModulationType modType = ModulationType.None, 
            Func<float, float> modFunc = null, int damage = 1, float bulletTranslationSpeed = 10f)
        {
            GameObject GO;
            if (!_secondaryBulletPool.TryPop(out GO))
            {
                throw new ObjectNotFoundException($"Secondary Bullet Pool is empty!");
            }
            GO.transform.position = position;
            GO.transform.rotation = rotation;
            WaveBullet WB = GO.GetComponent<WaveBullet>();
            var Index = _availableIndices.First().Value;
            WB.SetSpawnedWaveBulletSpecs(damage, bulletTranslationSpeed, Index, modType, waveFunc, modFunc);
            WB.OnSpawn();
            _spawnedBullets[Index] = GO;
            _availableIndices.Remove(Index);
            _spawnCount++;
            GO.SetActive(true);
            return GO;
        }

        public void ReturnToPool(GameObject bullet)
        {
            bullet.SetActive(false);
            bullet.transform.position = transform.position;
            bullet.transform.rotation = transform.rotation;
            var spwnIndex = bullet.GetComponent<RBullet>().SpawnInd;
            if (spwnIndex < 0)
            {
                throw new ArgumentException($"Bullet is not spawned");
            }
            if (spwnIndex > 0 && _spawnedBullets[spwnIndex] == null)
            {
                throw new ArgumentOutOfRangeException($"Bullet does not exist in the spawned list");
            }

            _spawnedBullets[spwnIndex] = null;
            _availableIndices.Add(spwnIndex, spwnIndex);
            _spawnCount--;
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
            bullet.GetComponent<IPooledBullet>().OnDespawn();
        }
        
        private void Update()
        {
            if (_spawnCount > 0 && _bulletUpdates == null)
            {
                _bulletUpdates = StartCoroutine(BulletUpdateCoroutine());
            }

            else if (_spawnCount <= 0 && _bulletUpdates != null)
            {
                StopCoroutine(_bulletUpdates);
                _bulletUpdates = null;
            }
        }

        private IEnumerator BulletUpdateCoroutine()
        {
            int i = 0;
            int j = 0;
            int maxSpawnableBullets = 2 * _bulletCap;
            while (true)
            {
                if (_spawnCount <= 0)
                    yield break;
                if (i >= maxSpawnableBullets)
                    i = 0;
                if (_availableIndices.ContainsKey(i))
                {
                    i++;
                    continue;
                }
                if (j >= _maxOpsPerFixedUpdate)
                {
                    j = 0;
                    yield return new WaitForFixedUpdate();
                }
                _spawnedBullets[i]?.GetComponent<IPooledBullet>().OnSpawnedUpdate();
                i++;
                j++;
            }
        }
    }
}
