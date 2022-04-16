using CoreResources.Utils;

namespace GameResources.Bullet
{
    public class TriBullet : RSpecBullet
    {
        public override void OnEnable()
        {
            WaveFunction = WaveGenerator.Tri;
            base.OnEnable();
        }
    }
}