using GameResources.Character;
using GameResources.Events;

namespace GameResources.Enemy.Mook
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