using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using CoreResources.Utils;
using CoreResources.Utils.Singletons;
using GameResources.Enemy;
using GameResources.Enemy.Mook;
using GameResources.Events;
using UnityEngine;

namespace GameResources.Character
{
    // This class will deal with spawning and despawning all required characters, including enemies, player and the boss
    public class CharacterPoolManager : MonoBehaviorSingleton<CharacterPoolManager>
    {
        public GameObject PlayerShip { get; private set; }
        public GameObject Boss { get; private set; }
        private GameObject ShootingMook;
        private GameObject KamikazeMook;
        private bool _playerShipSpawned => PlayerShip.activeSelf; // Change these later
        private bool _bossSpawned => Boss.activeSelf;
        private const int MaxSpawnedMooks = 10;
        // private Stack<GameObject> _mookPool;
        private Dictionary<CharacterType, Stack<GameObject>> _mookPool;
        private GameObject[] _spawnedMooks;
        private int _mookSpawnCount = 0;
        private Dictionary<int, int> _availableIndices; // corresponds to the spawned mook array
        private Coroutine _mookUpdateCoroutine;
        
        // Change this later
        private readonly Vector3 _playerSpawnPoint = new Vector3(0, 4, -4);
        private readonly Vector3 _mookSpawnPoint = new Vector3(5, 10, -4);
        private readonly Vector3 _bossSpawnPoint = new Vector3(0, 10, -4);
        private Quaternion _defaultPlayerSpawnRotation;
        private Quaternion _defaultBossSpawnRotation;

        protected override void InitSingleton()
        {
            base.InitSingleton();
            // _playerShipSpawned = false;
            // _bossSpawned = false;
            _mookSpawnCount = 0;
            _mookPool = new Dictionary<CharacterType, Stack<GameObject>>();
            _spawnedMooks = new GameObject[MaxSpawnedMooks];
            _availableIndices = new Dictionary<int, int>();
            _defaultPlayerSpawnRotation = Quaternion.Euler(-90, 0, 0);
            _defaultBossSpawnRotation = Quaternion.Euler(-90, 0, 0);

            LoadCharacterPools();
            InstantiateCharacterPools();
        }

        private void LoadCharacterPools()
        {
            PlayerShip = AppHandler.AssetManager.LoadAsset<GameObject>("Player");
            Boss = AppHandler.AssetManager.LoadAsset<GameObject>("Boss1");
            ShootingMook = AppHandler.AssetManager.LoadAsset<GameObject>("ShootingMook");
            KamikazeMook = AppHandler.AssetManager.LoadAsset<GameObject>("KamikazeMook");
        }

        private void InstantiateCharacterPools()
        {
            PlayerShip = Instantiate(PlayerShip, _playerSpawnPoint, _defaultPlayerSpawnRotation, transform);
            Boss = Instantiate(Boss, _bossSpawnPoint, Quaternion.identity, transform);
            PlayerShip.SetActive(false);
            Boss.SetActive(false);
            
            _mookPool.Add(CharacterType.ShootingMook, new Stack<GameObject>());
            _mookPool.Add(CharacterType.KamikazeMook, new Stack<GameObject>());

            for (int i = 0; i < MaxSpawnedMooks; i++)
            {
                var GO = Instantiate(ShootingMook, transform.position, transform.rotation, transform);
                GO.SetActive(false);
                _mookPool[CharacterType.ShootingMook].Push(GO);
                GO = Instantiate(KamikazeMook, transform.position, transform.rotation, transform);
                GO.SetActive(false);
                _mookPool[CharacterType.KamikazeMook].Push(GO);
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
                PlayerShip.transform.rotation = _defaultPlayerSpawnRotation;
            }
            
            PlayerShip.SetActive(true);
            PlayerShip.GetComponent<IPooledCharacter>().OnSpawn();
            REvent_PlayerSpawned.Dispatch(PlayerShip.transform);
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
                Boss.transform.rotation = _defaultBossSpawnRotation;
            }

            Boss.SetActive(true);
            Boss.GetComponent<IPooledCharacter>().OnSpawn();
            return Boss;
        }

        public GameObject SpawnMook(CharacterType MookType, Vector3 position, Quaternion rotation)
        {
            if (MookType == CharacterType.Boss || MookType == CharacterType.PlayerShip)
            {
                Debug.LogError($"Incorrect Character type {MookType}");
                return null;
            }
            
            GameObject GO;
            if (!_mookPool[MookType].TryPop(out GO))
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
                case CharacterType.ShootingMook:
                case CharacterType.KamikazeMook:
                    var spawnIndex = character.GetComponent<MookController>().SpawnInd;
                    if (spawnIndex < 0)
                        throw new ArgumentException($"Mook is not spawned");
                    if (spawnIndex > 0 && _spawnedMooks[spawnIndex] == null)
                    {
                        throw new ArgumentOutOfRangeException($"Mook does not exist in the list of spawned objects");
                    }
                    
                    _spawnedMooks[spawnIndex] = null;
                    _availableIndices.Add(spawnIndex, spawnIndex);
                    _mookSpawnCount--;
                    _mookPool[charType].Push(character);
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

        /*private void Update()
        {
            if (_mookSpawnCount > 0 && _mookUpdateCoroutine == null)
            {
                _mookUpdateCoroutine = StartCoroutine(MookUpdateCoroutine());
            }
            else if (_mookSpawnCount <= 0 && _mookUpdateCoroutine != null)
            {
                StopCoroutine(_mookUpdateCoroutine);
                _mookUpdateCoroutine = null;
            }
        }*/

        /*private IEnumerator MookUpdateCoroutine()
        {
            int i = 0;
            while (true)
            {
                yield return new WaitForFixedUpdate();
                if (i >= MaxSpawnedMooks)
                {
                    i = 0;
                }

                if (_availableIndices.ContainsKey(i))
                {
                    i++;
                    continue;
                }
                // Added the bash operator in case the mook is returned to pool during the coroutine (race condition)
                _spawnedMooks[i]?.GetComponent<IPooledCharacter>().OnSpawnedUpdate(); 
                i++;
            }
        }*/
    }
}