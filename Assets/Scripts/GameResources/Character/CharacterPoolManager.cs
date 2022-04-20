using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using CoreResources.Utils.Singletons;
using GameResources.Enemy;
using UnityEngine;

namespace GameResources.Character
{
    // This class will deal with spawning and despawning all required characters, including enemies, player and the boss
    public class CharacterPoolManager : MonoBehaviorSingleton<CharacterPoolManager>
    {
        public GameObject PlayerShip { get; private set; }
        public GameObject Boss { get; private set; }
        private GameObject Mook1;
        private bool _playerShipSpawned => PlayerShip.activeSelf; // Change these later
        private bool _bossSpawned => PlayerShip.activeSelf;
        private const int MaxSpawnedMooks = 10;
        private Stack<GameObject> _mookPool;
        private GameObject[] _spawnedMooks;
        private int _mookSpawnCount = 0;
        private Dictionary<int, int> _availableIndices;
        private Coroutine _mookUpdateCoroutine;
        
        // Change this later
        private readonly Vector3 _playerSpawnPoint = new Vector3(0, 4, -4);
        private readonly Vector3 _mook1SpawnPoint = new Vector3(5, 10, -4);
        private readonly Vector3 _bossSpawnPoint = new Vector3(0, 10, -4);
        private Quaternion _playerSpawnRotation;

        protected override void InitSingleton()
        {
            base.InitSingleton();
            // _playerShipSpawned = false;
            // _bossSpawned = false;
            _mookSpawnCount = 0;
            _mookPool = new Stack<GameObject>();
            _spawnedMooks = new GameObject[MaxSpawnedMooks];
            _availableIndices = new Dictionary<int, int>();
            _playerSpawnRotation = Quaternion.Euler(-90, 0, 0);

            LoadCharacterPools();
            InstantiateCharacterPools();

            SpawnPlayerShip(Vector3.zero, Quaternion.identity, true);
            SpawnMook(new Vector3(10, 10, -4), Quaternion.identity);
        }

        private void LoadCharacterPools()
        {
            PlayerShip = AppHandler.AssetManager.LoadAsset<GameObject>("Player");
            Boss = AppHandler.AssetManager.LoadAsset<GameObject>("Boss1");
            Mook1 = AppHandler.AssetManager.LoadAsset<GameObject>("Mook1");
        }

        private void InstantiateCharacterPools()
        {
            PlayerShip = Instantiate(PlayerShip, _playerSpawnPoint, _playerSpawnRotation, transform);
            Boss = Instantiate(Boss, _bossSpawnPoint, Quaternion.identity, transform);
            PlayerShip.SetActive(false);
            Boss.SetActive(false);

            for (int i = 0; i < MaxSpawnedMooks; i++)
            {
                var GO = Instantiate(Mook1, transform.position, transform.rotation, transform);
                GO.SetActive(false);
                _mookPool.Push(GO);
                _availableIndices.Add(i, i);
            }
        }

        public GameObject SpawnPlayerShip(Vector3 position, Quaternion rotation, bool defaultTransform = false)
        {
            if (PlayerShip == null)
                throw new NullReferenceException($"PlayerShip object is not instantiated yet");
            if (_playerShipSpawned)
                throw new Exception($"Player ship is already spawned");
            if (!defaultTransform)
            {
                PlayerShip.transform.position = position;
                PlayerShip.transform.rotation = rotation;
            }
            else
            {
                PlayerShip.transform.position = _playerSpawnPoint;
                PlayerShip.transform.rotation = _playerSpawnRotation;
            }
            
            PlayerShip.SetActive(true);
            PlayerShip.GetComponent<IPooledCharacter>().OnSpawn();
            return PlayerShip;
        }

        public GameObject SpawnBoss(Vector3 position, Quaternion rotation, bool defaultTransform = false)
        {
            if (Boss == null)
                throw new NullReferenceException($"Boss object is not instantiated yet");
            if (_bossSpawned)
                throw new Exception($"Boss is already spawned");
            if (!defaultTransform)
            {
                Boss.transform.position = position;
                Boss.transform.rotation = rotation;
            }
            else
            {
                Boss.transform.position = _bossSpawnPoint;
                Boss.transform.rotation = Quaternion.identity;
            }

            Boss.SetActive(true);
            Boss.GetComponent<IPooledCharacter>().OnSpawn();
            return Boss;
        }

        public GameObject SpawnMook(Vector3 position, Quaternion rotation)
        {
            GameObject GO;
            if (!_mookPool.TryPop(out GO))
                throw new ObjectNotFoundException($"All available mooks are spawned right now. Try again later");
            GO.transform.position = position;
            GO.transform.rotation = rotation;
            var Index = _availableIndices.First().Value;
            _availableIndices.Remove(Index);
            _spawnedMooks[Index] = GO;
            _mookSpawnCount++;
            GO.SetActive(true);
            GO.GetComponent<IPooledCharacter>().OnSpawn();
            return GO;
        }

        public void ReturnToPool(GameObject character)
        {
            character.SetActive(false);
            character.transform.position = transform.position;
            character.transform.rotation = transform.rotation;
            var charController = character.GetComponent<RCharacterController>();
            var charType = charController.charType;
            switch (charType)
            {
                case CharacterType.PlayerShip:
                    // Any logic necessary
                    break;
                case CharacterType.Mook:
                    var spawnIndex = character.GetComponent<MookController>().SpawnInd;
                    if (spawnIndex < 0)
                        throw new ArgumentException($"Mook is not spawned");
                    if (spawnIndex > 0 && _spawnedMooks[spawnIndex] == null)
                    {
                        throw new ArgumentOutOfRangeException($"Mook does not exist in the list of spawned objects");
                    }

                    _spawnedMooks[spawnIndex] = null;
                    _availableIndices.Remove(spawnIndex);
                    _mookSpawnCount--;
                    _mookPool.Push(character);
                    break;
                case CharacterType.Boss:
                    // Any logic necessary
                    break;
                default:
                    throw new ArgumentOutOfRangeException(
                        $"Character type not assigned or given a value that isn't accounted for");
            }
            
            charController.OnDespawn();
        }

        private void Update()
        {
            if (_mookSpawnCount > 0 && _mookUpdateCoroutine == null)
            {
                _mookUpdateCoroutine = StartCoroutine(MookUpdateCoroutine());
            }
            else if (_mookSpawnCount <= 0 && _mookUpdateCoroutine == null)
            {
                StopCoroutine(_mookUpdateCoroutine);
                _mookUpdateCoroutine = null;
            }
        }

        private IEnumerator MookUpdateCoroutine()
        {
            int i = 0;
            while (true)
            {
                yield return null;
                if (i >= MaxSpawnedMooks)
                {
                    i = 0;
                }
                if (_availableIndices.ContainsKey(i))
                {
                    i++;
                    continue;
                }
                _spawnedMooks[i].GetComponent<IPooledCharacter>().OnSpawnedUpdate();
                i++;
            }
        }
    }
}