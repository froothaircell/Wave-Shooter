using System;
using GameResources.Character;
using GameResources.Gun;
using UnityEngine;

namespace GameResources.Player
{
    public class GunController : MonoBehaviour, ICharacterComponent
    {
        private PlayerGun _playerGun;

        public void OnInit()
        {
            _playerGun = GetComponentInChildren<PlayerGun>();
            _playerGun.InitGun();
        }

        public void OnUpdate()
        {
            if (Input.GetButtonDown("Fire1"))
            {
                _playerGun.FireBullet();
            }

            if (Input.GetButtonDown("Fire2"))
            {
                _playerGun.FireSpecialBullet();
            }
        }

        public void OnDeInit()
        {
            _playerGun = null;
        }
    }
}
