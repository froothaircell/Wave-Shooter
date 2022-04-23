using CoreResources.Handlers.EventHandler;
using UnityEngine;

namespace GameResources.Events
{
    public class REvent_MookDeath : REvent
    {
        public Transform EnemyTransform;

        public static void Dispatch(Transform enemyTransform)
        {
            var evt = Get<REvent_MookDeath>();
            evt.EnemyTransform = enemyTransform;
            AppHandler.EventManager.Dispatch(evt);
        }
    }
    
    public class REvent_BossDeath : REvent
    {
        public Transform BossTransform;

        public static void Dispatch(Transform bossTransform)
        {
            var evt = Get<REvent_BossDeath>();
            evt.BossTransform = bossTransform;
            AppHandler.EventManager.Dispatch(evt);
        }
    }
}