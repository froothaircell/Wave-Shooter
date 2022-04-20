using CoreResources.Handlers.EventHandler;
using CoreResources.Pool;
using CoreResources.Utils.ResourceLoader;
using CoreResources.Utils.Singletons;
using GameResources.Bullet;
using GameResources.Character;
using UnityEditor;
using UnityEngine;

namespace GameResources
{
    public class AppHandler : MonoBehaviorSingleton<AppHandler>
    {
        public static TypePool EventPool = new("EventPool");
        public static REventHandler EventManager;
        public static AssetLoader AssetManager;
        public static CharacterPoolManager CharacterManager;
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
            AssetManager = AssetLoader.SetInstanceType<AssetLoader>();
            CharacterManager = CharacterPoolManager.Instance;
            BulletManager = BulletPoolManager.Instance;
            
            CharacterManager.SpawnPlayerShip(Vector3.zero, Quaternion.identity, true);
            CharacterManager.SpawnMook(new Vector3(10, 10, -4), Quaternion.identity);
        }
    }
}