using GameResources.Character;
using GameResources.Events;

namespace GameResources.Enemy.Mook
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