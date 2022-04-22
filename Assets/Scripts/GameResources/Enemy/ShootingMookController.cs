using CoreResources.Handlers.EventHandler;
using GameResources.Character;
using GameResources.Events;
using UnityEngine;

namespace GameResources.Enemy
{
    public class ShootingMookController : MookController
    {
        public override void OnSpawn()
        {
            base.OnSpawn();
            charType = CharacterType.ShootingMook;

            AppHandler.EventManager.Subscribe<REvent_MookDeath>(OnCharacterDeath, _disposables);
        }
    }
}