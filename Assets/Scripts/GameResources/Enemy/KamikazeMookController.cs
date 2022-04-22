using CoreResources.Handlers.EventHandler;
using GameResources.Character;
using GameResources.Events;

namespace GameResources.Enemy
{
    public class KamikazeMookController : MookController
    {
        public override void OnSpawn()
        {
            base.OnSpawn();
            charType = CharacterType.KamikazeMook;

            AppHandler.EventManager.Subscribe<REvent_MookDeath>(OnCharacterDeath, _disposables);
        }
    }
}