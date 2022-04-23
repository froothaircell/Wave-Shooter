using System.Collections;
using GameResources.Character;
using UnityEngine;

namespace GameResources.Enemy.Boss
{
    public class BossGunController : MonoBehaviour, ICharacterComponent
    {
        private BossGun[] _bossGuns;
        private Coroutine _shootingCoroutine;
        private Transform _playerShip;
        private float _fireRate = 3f;
        
        public void OnInit()
        {
            _bossGuns = GetComponentsInChildren<BossGun>();
            foreach(var bossGun in _bossGuns)
                bossGun.InitGun();
            _playerShip = AppHandler.CharacterManager.PlayerShip.transform;
            _shootingCoroutine = StartCoroutine(ShootingCoroutine());
        }

        public void OnUpdate()
        {
            
        }

        public void OnDeInit()
        {
            if (_shootingCoroutine != null)
            {
                StopCoroutine(_shootingCoroutine);
                _shootingCoroutine = null;
            }

            _bossGuns = null;
        }

        private IEnumerator ShootingCoroutine()
        {
            while (true)
            {
                if (!_playerShip.gameObject.activeSelf)
                {
                    OnDeInit();
                    yield break;
                }

                foreach (var bossGun in _bossGuns)
                {
                    bossGun.FireBullet();
                }

                yield return new WaitForSeconds(_fireRate);
            }
        }
    }
}