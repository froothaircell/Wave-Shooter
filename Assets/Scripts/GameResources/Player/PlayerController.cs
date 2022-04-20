using System;
using System.Runtime.Remoting.Messaging;
using CoreResources.Handlers.EventHandler;
using GameResources.Character;
using GameResources.Events;
using GameResources.Gun;
using UnityEngine;

namespace GameResources.Player
{
    public class PlayerController : RCharacterController
    {
        public override void OnSpawn()
        {
            base.OnSpawn();
            charType = CharacterType.PlayerShip;

            AppHandler.EventManager.Subscribe<REvent_PlayerDeath>(OnCharacterDeath, _disposables);
        }

        private void Update()
        {
            foreach (var component in _components)
            {
                component.OnUpdate();
            }
        }

        public override void OnSpawnedUpdate()
        {
            // Keeping this empty for now but if performance of the PoolManager suffices we'll put the update stuff here
        }

        public override void OnCharacterDeath(REvent evt)
        {
            AppHandler.CharacterManager.ReturnToPool(gameObject);
        }
    }
}