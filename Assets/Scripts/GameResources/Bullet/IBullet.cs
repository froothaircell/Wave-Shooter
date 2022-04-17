namespace GameResources.Bullet
{
    public interface IBullet
    {
        int Damage { get; }
        
        void OnInit();
        void OnUpdate();
        void OnDeInit();
    }
}