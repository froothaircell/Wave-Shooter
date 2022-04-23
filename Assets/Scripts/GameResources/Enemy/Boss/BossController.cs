using CoreResources.Handlers.EventHandler;
using GameResources.Character;
using GameResources.Events;

namespace GameResources.Enemy.Boss
{
    public class BossController : RCharacterController
    {
        public override void OnSpawn()
        {
            base.OnSpawn();
            charType = CharacterType.Boss;

            AppHandler.EventManager.Subscribe<REvent_BossDeath>(OnCharacterDeath, _disposables);
        }

        public override void OnCharacterDeath(REvent evt)
        {
            AppHandler.CharacterManager.ReturnToPool(gameObject);
        }
    }
}