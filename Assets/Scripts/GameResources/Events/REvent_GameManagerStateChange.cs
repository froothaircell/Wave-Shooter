using CoreResources.Handlers.EventHandler;

namespace GameResources.Events
{
    public class REvent_GameQuit : REvent
    {
        public static void Dispatch()
        {
            var evt = Get<REvent_GameQuit>();
            AppHandler.EventManager.Dispatch(evt);
        }
    }
}