using CoreResources.Handlers.EventHandler;

namespace GameResources.Character
{
    public interface IPooledCharacter
    {
        void OnSpawn();
        void OnSpawnedUpdate();
        void OnDespawn();
        void OnCharacterDeath(REvent evt);
    }
}