using CoreResources.Handlers.EventHandler;
using GameResources.Character;

namespace GameResources.Enemy.Mook
{
    public abstract class MookController : RCharacterController
    {
        public int SpawnInd { get; private set; }
        
        public void SetSpawnIndex(int val)
        {
            SpawnInd = val;
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