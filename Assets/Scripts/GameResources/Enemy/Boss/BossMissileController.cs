using System;
using System.Collections;
using System.Collections.Generic;
using CoreResources.Handlers.EventHandler;
using CoreResources.Utils.Disposables;
using GameResources.Character;
using GameResources.Events;
using UnityEngine;

namespace GameResources.Enemy.Boss
{
    public class BossMissileController : MonoBehaviour, ICharacterComponent
    {
        public Transform[] missileSlots;
        public float missileFireDelay = 10f;

        private BossMissile[] _missiles;
        private Coroutine _missileFiringCoroutine;
        private Coroutine _missileLoadingCoroutine;
        private List<IDisposable> _disposables;

        private bool _missilesLoaded => _missiles != null;

        public void OnInit()
        {
            _disposables = new List<IDisposable>();
            AppHandler.EventManager.Subscribe<REvent_BossHalfHealth>(OnHalfHealth, _disposables);
            _missileLoadingCoroutine = StartCoroutine(MissileLoadingCoroutine());
        }

        public void OnUpdate()
        {
            
        }

        public void OnDeInit()
        {
            if (_missileFiringCoroutine != null)
            {
                StopCoroutine(_missileFiringCoroutine);
                _missileFiringCoroutine = null;
            }
            
            if (_missileLoadingCoroutine != null)
            {
                StopCoroutine(_missileLoadingCoroutine);
                _missileLoadingCoroutine = null;
            }
            
            _disposables.ClearDisposables();
            _disposables = null;
        }

        private void OnHalfHealth(REvent evt)
        {
            if (_missileFiringCoroutine == null)
            {
                _missileFiringCoroutine = StartCoroutine(MissileFiringCoroutine());
            }
        }

        private IEnumerator MissileLoadingCoroutine()
        {
            while (true)
            {
                if (_missilesLoaded)
                {
                    yield return null;
                    continue;
                }
                
                _missiles = AppHandler.BulletManager.LoadMissilesToSlots(missileSlots);
            }
        }

        private IEnumerator MissileFiringCoroutine()
        {
            while (true)
            {
                // _missiles = AppHandler.BulletManager.LoadMissilesToSlots(missileSlots);

                if (!_missilesLoaded) // Checks if missiles are loaded
                {
                    yield return null;
                    continue;
                }

                foreach (var missile in _missiles)
                {
                    missile.FireMissile();
                }

                yield return new WaitForSeconds(missileFireDelay);
                _missiles = null;
            }
        }
    }
}