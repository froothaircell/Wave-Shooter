using CoreResources.Handlers.EventHandler;
using GameResources.Character;
using GameResources.Events;
using UnityEngine;

namespace GameResources.Enemy
{
    public class MookController : RCharacterController
    {
        public int SpawnInd { get; private set; }

        public void SetSpawnIndex(int val)
        {
            SpawnInd = val;
        }
        
        public override void OnSpawn()
        {
            base.OnSpawn();
            charType = CharacterType.Mook;

            AppHandler.EventManager.Subscribe<REvent_MookDeath>(OnCharacterDeath, _disposables);
        }

        public override void OnDespawn()
        {
            base.OnDespawn();
            SpawnInd = -1;
        }

        public override void OnCharacterDeath(REvent evt)
        {
            AppHandler.CharacterManager.ReturnToPool(gameObject);
        }
    }
}