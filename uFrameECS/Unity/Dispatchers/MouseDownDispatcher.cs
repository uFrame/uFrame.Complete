using uFrame.Attributes;
using uFrame.ECS.Components;

namespace uFrame.ECS.UnityUtilities
{
    [UFrameEventDispatcher("On Mouse Down"), uFrameCategory("Unity Messages")]
    public class MouseDownDispatcher : EcsDispatcher
    {
        public void OnMouseDown()
        {
            Publish(this);
        }
    }
}