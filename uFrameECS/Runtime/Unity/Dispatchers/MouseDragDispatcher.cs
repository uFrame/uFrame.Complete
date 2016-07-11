using uFrame.Attributes;
using uFrame.ECS.Components;

namespace uFrame.ECS.UnityUtilities
{
    [UFrameEventDispatcher("On Mouse Drag"), uFrameCategory("Unity Messages")]
    public class MouseDragDispatcher : EcsDispatcher
    {
        public void OnMouseDrag()
        {
            Publish(this);
        }
    }
}