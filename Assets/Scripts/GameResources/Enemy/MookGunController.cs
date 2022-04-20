using System.Collections;
using GameResources.Character;
using UnityEngine;

namespace GameResources.Enemy
{
    public class MookGunController : MonoBehaviour, ICharacterComponent
    {
        private MookGun _mookGun;
        private Coroutine _shootingCoroutine;
        private Transform _playerShip;
        
        public void OnInit()
        {
            _mookGun = GetComponentInChildren<MookGun>();
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
            _mookGun = null;
        }

        private IEnumerator ShootingCoroutine()
        {
            while (true)
            {
                Vector3 lookDir = (_playerShip.position - transform.position).normalized;
                transform.rotation = Quaternion.LookRotation(lookDir, -Vector3.forward);
            }
        }
    }
}