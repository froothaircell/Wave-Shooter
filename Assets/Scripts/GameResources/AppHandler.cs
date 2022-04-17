using CoreResources.Handlers.EventHandler;
using CoreResources.Pool;
using CoreResources.Utils.ResourceLoader;
using CoreResources.Utils.Singletons;
using GameResources.Bullet;
using UnityEditor;

namespace GameResources
{
    public class AppHandler : MonoBehaviorSingleton<AppHandler>
    {
        public static TypePool EventPool = new("EventPool");
        public static REventHandler EventManager;
        public static AssetLoader AssetHandler;
        public static BulletPoolManager BulletManager;

        protected override void Awake()
        {
            base.Awake();
            if (GetInstanceID() == Instance.GetInstanceID())
            {
                Initialize();
            }
        }

        private void Initialize()
        {
            // Make the core services available before starting the game related stuff
            EventManager = REventHandler.SetInstanceType<REventHandler>();
            AssetHandler = AssetLoader.SetInstanceType<AssetLoader>();
            BulletManager = BulletPoolManager.Instance;
        }
    }
}