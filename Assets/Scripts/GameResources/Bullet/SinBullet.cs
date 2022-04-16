using CoreResources.Utils;

namespace GameResources.Bullet
{
    public class SinBullet : RSpecBullet
    {
        public override void OnEnable()
        {
            WaveFunction = WaveGenerator.Sin;
            base.OnEnable();
        }
    }
}