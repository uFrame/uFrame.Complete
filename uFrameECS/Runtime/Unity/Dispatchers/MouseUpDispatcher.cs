using uFrame.Attributes;
using uFrame.ECS.Components;

namespace uFrame.ECS.UnityUtilities
{
    [UFrameEventDispatcher("On Mouse Up"), uFrameCategory("Unity Messages")]
    public class MouseUpDispatcher : EcsDispatcher
    {
        public void OnMouseUp()
        {
            Publish(this);
        }
    }
}