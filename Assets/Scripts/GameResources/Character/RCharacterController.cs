using System;
using System.Collections.Generic;
using CoreResources.Handlers.EventHandler;
using CoreResources.Utils.Disposables;
using UnityEngine;
using UnityEngine.Serialization;

namespace GameResources.Character
{
    public enum CharacterType
    {
        PlayerShip = 0,
        ShootingMook = 1,
        KamikazeMook = 2,
        Boss = 3
    }
    
    public abstract class RCharacterController : MonoBehaviour, IPooledCharacter
    {
        public CharacterType charType;
        protected ICharacterComponent[] _components;
        protected List<IDisposable> _disposables;
        
        public virtual void OnSpawn()
        {
            _components = GetComponentsInChildren<ICharacterComponent>();
            foreach (var component in _components)
            {
                component.OnInit();
            }
            
            if(_disposables == null)
                _disposables = new List<IDisposable>();
        }

        public virtual void Update()
        {
            foreach (var component in _components)
            {
                component.OnUpdate();
            }
        }

        public virtual void OnDespawn()
        {
            foreach (var component in _components)
            {
                component.OnDeInit();
            }
            _disposables.ClearDisposables();
        }

        public abstract void OnCharacterDeath(REvent evt);
    }
}