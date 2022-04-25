namespace GameResources.Projectiles
{
    public interface IPooledProjectile
    {
        int Damage { get; }
        
        void OnSpawn();
        void OnSpawnedUpdate();
        void OnDespawn();
    }
}