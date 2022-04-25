using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using BulletFury;
using CoreResources.Utils.Singletons;
using GameResources.Enemy.Boss;
using GameResources.Projectiles.Missile;
using UnityEngine;

namespace GameResources.Projectiles.Bullet
{
    public class ProjectilePoolManager : MonoBehaviorSingleton<ProjectilePoolManager>
    {
        private const int _bulletCap = 250;
        private const int _missileCap = 6;
        private const int _maxOpsPerFixedUpdate = 50; // This ensures batching during a coroutine
        
        // Game objects to load from resources
        private GameObject _primaryBullet;
        private GameObject _secondaryBullet;
        private GameObject _bossMissile;
        
        // Projectile pools
        private Stack<GameObject> _primaryBulletPool;
        private Stack<GameObject> _secondaryBulletPool;
        private Stack<GameObject> _bossMissilePool;
        
        // Lists for tracking spawned projectiles
        private GameObject[] _spawnedBullets;
        private List<GameObject> _spawnedMissiles;
        private Dictionary<int, int> _availableBulletIndices;
        private int _spawnCount = 0;
        
        // To manage bullet updates for the player
        private Coroutine _bulletUpdates;
        
        // For defining the projectile damage
        private int _mookLevel = 3;
        private int _bossLevel = 10;
        private int MookBulletDamage => _mookLevel; // Add nuance to the equation later
        private int BossBulletDamage => _bossLevel;
        private float BossBulletTranslationSpeed = 10f;
        private int BossMissileDamage => _bossLevel;
        private float BossMissileSpeed = 10f;
        private float BossMissileTrackingFactor = 0.01f;
        private float BossMissileBulletFirePeriod = 0.5f;
        
        // Reference to enemy bullet managers
        private BulletManager _mookBulletManager;
        private BulletManager _mookExplosionManager;

        protected override void InitSingleton()
        {
            base.InitSingleton();
            _primaryBulletPool = new Stack<GameObject>();
            _secondaryBulletPool = new Stack<GameObject>();
            _bossMissilePool = new Stack<GameObject>();
            _spawnedBullets = new GameObject[500];
            _spawnedMissiles = new List<GameObject>();
            _availableBulletIndices = new Dictionary<int, int>();
            _spawnCount = 0;

            // Setting up Enemy bullet managers
            LoadEnemyBulletManagers();
            
            LoadBullets();
            InstantiateBulletPools();

            LoadMissiles();
            InstantiateMissilePools();
        }

        #region BulletBasedFunctions
        private void LoadEnemyBulletManagers()
        {
            var GO = AppHandler.AssetManager.LoadAsset<GameObject>("MookBulletManager");
            GO = Instantiate(GO, transform.position, transform.rotation, transform);
            _mookBulletManager = GO.GetComponent<BulletManager>();
            _mookBulletManager.GetBulletSettings().SetDamage(MookBulletDamage);
            GO = AppHandler.AssetManager.LoadAsset<GameObject>("MookExplosionManager");
            GO = Instantiate(GO, transform.position, transform.rotation, transform);
            _mookExplosionManager = GO.GetComponent<BulletManager>();
            _mookExplosionManager.GetBulletSettings().SetDamage(MookBulletDamage);
        }

        private void LoadBullets()
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
                _availableBulletIndices.Add(i, i);
            }

            for (int i = _bulletCap; i < (2 * _bulletCap); i++)
            {
                _availableBulletIndices.Add(i, i);
            }
        }
        
        public void SpawnEnemyExplosion(Vector3 position, Vector3 forward)
        {
            _mookExplosionManager.Spawn(position, forward);
        }

        public void SpawnEnemyBullet(Vector3 position, Vector3 forward)
        {
            _mookBulletManager.Spawn(position, forward);
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
            var Index = _availableBulletIndices.First().Value;
            BB.SetSpawnedBulletSpecs(damage, bulletTranslationSpeed, Index);
            BB.OnSpawn();
            _spawnedBullets[Index] = GO;
            _availableBulletIndices.Remove(Index);
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
            var Index = _availableBulletIndices.First().Value;
            WB.SetSpawnedWaveBulletSpecs(damage, bulletTranslationSpeed, Index, modType, waveFunc, modFunc);
            WB.OnSpawn();
            _spawnedBullets[Index] = GO;
            _availableBulletIndices.Remove(Index);
            _spawnCount++;
            GO.SetActive(true);
            return GO;
        }

        public void ReturnBulletToPool(GameObject bullet)
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
            _availableBulletIndices.Add(spwnIndex, spwnIndex);
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
            bullet.GetComponent<IPooledProjectile>().OnDespawn();
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
                if (_availableBulletIndices.ContainsKey(i))
                {
                    i++;
                    continue;
                }
                if (j >= _maxOpsPerFixedUpdate)
                {
                    j = 0;
                    yield return new WaitForFixedUpdate();
                }
                _spawnedBullets[i]?.GetComponent<IPooledProjectile>().OnSpawnedUpdate();
                i++;
                j++;
            }
        }
        #endregion

        #region MissileBasedFunctions
        private void LoadMissiles()
        {
            _bossMissile = AppHandler.AssetManager.LoadAsset<GameObject>("Missile");
        }

        private void InstantiateMissilePools()
        {
            for (int i = 0; i < _missileCap; i++)
            {
                var missile = Instantiate(_bossMissile, transform.position, transform.rotation, transform);
                missile.SetActive(false);
                var missileStats = missile.GetComponent<BossMissile>();
                if (missileStats != null)
                {
                    missileStats.SetMissileStats(BossMissileDamage, BossMissileSpeed,
                        BossMissileTrackingFactor, BossBulletDamage, BossMissileBulletFirePeriod,
                        BossBulletTranslationSpeed);
                }
                _bossMissilePool.Push(missile);
            }
        }
        
        public BossMissile[] LoadMissilesToSlots(Transform[] missileSlots)
        {
            if (_bossMissilePool.Count < missileSlots.Length)
                return null;
            var resMissiles = new BossMissile[missileSlots.Length];
            int i = 0;
            foreach (var missileSlot in missileSlots)
            {
                var missile = _bossMissilePool.Pop();
                // var joint = missile.GetComponent<FixedJoint>();
                // var targetRB = missileSlot.GetComponentInParent<Rigidbody>();
                // joint.connectedBody = targetRB;
                _spawnedMissiles.Add(missile);
                resMissiles[i] = missile.GetComponent<BossMissile>();
                resMissiles[i].OnSpawn();
                resMissiles[i].LoadMissile(missileSlot);
                missile.SetActive(true);
                i++;
            }

            return resMissiles;
        }

        public void ReturnMissileToPool(GameObject missile)
        {
            var missileController = missile.transform.GetComponent<BossMissile>();
            if (missileController.CurrentMissileState != MissileState.Firing)
            {
                Debug.LogError($"Missile is not in the firing state. Cannot be returned to pool!");
                return;
            }
            missileController.OnDespawn();
            missile.SetActive(false);
            missile.transform.position = transform.position;
            missile.transform.rotation = transform.rotation;
            _spawnedMissiles.Remove(missile);
            _bossMissilePool.Push(missile);
        }
        #endregion



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

    }
}
