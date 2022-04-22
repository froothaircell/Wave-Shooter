using CoreResources.Handlers.EventHandler;
using GameResources.Character;
using GameResources.Events;

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

        public override void Update()
        {
            foreach (var component in _components)
            {
                component.OnUpdate();
            }
        }

        public override void OnCharacterDeath(REvent evt)
        {
            AppHandler.CharacterManager.ReturnToPool(gameObject);
        }
    }
}