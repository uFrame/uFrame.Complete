using uFrame.Attributes;
using UnityEngine;

namespace uFrame.ECS.Actions
{
    [ActionLibrary, uFrameCategory("Debug")]
    public class Log : UFAction
    {
        [In] public string Message;
        public override void Execute()
        {
            Debug.Log(Message);
        }
    }
}