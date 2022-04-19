using CoreResources.Handlers.EventHandler;
using UnityEngine;

namespace GameResources.Events
{
    public class REvent_PlayerDeath : REvent
    {
        public Transform LastPosition;

        public static void Dispatch(Transform shipTransform)
        {
            var evt = Get<REvent_PlayerDeath>();
            evt.LastPosition = shipTransform;
            AppHandler.EventManager.Dispatch(evt);
        }
    }
}