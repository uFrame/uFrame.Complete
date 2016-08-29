using uFrame.Attributes;

namespace uFrame.ECS.Actions
{
    [ActionLibrary, uFrameCategory("Log", "Debug")]
    public static class DebugLibrary
    {
        [ActionTitle("Log Message")]
        public static void LogMessage(object message)
        {
            UnityEngine.Debug.Log(message);
        }
    }
}