using uFrame.Attributes;
using uFrame.ECS.Components;
using UnityEngine.EventSystems;

namespace uFrame.ECS.UnityUtilities
{
    [UFrameEventDispatcher("On Move"), uFrameCategory("uGUI")]
    public class MoveDispatcher : EcsDispatcher, IMoveHandler
    {
        public AxisEventData AxisData { get; set; }


        public void OnMove(AxisEventData eventData)
        {
            Publish(this);
        }
    }
}