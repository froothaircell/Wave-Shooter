namespace GameResources.Bullet
{
    public interface IPooledBullet
    {
        int Damage { get; }
        
        void OnSpawn();
        void OnSpawnedUpdate();
        void OnDespawn();
    }
}