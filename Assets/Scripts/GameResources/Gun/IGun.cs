namespace GameResources.Gun
{
    public interface IGun
    {
        float PrimaryFireRate { get; }

        void OnInit();
        void OnDeInit();
    }
}