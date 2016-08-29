using uFrame.Attributes;
using uFrame.ECS.Components;

namespace uFrame.ECS.UnityUtilities
{
    [UFrameEventDispatcher("On Mouse Enter"), uFrameCategory("Unity Messages")]
    public class MouseEnterDispatcher : EcsDispatcher
    {
        public void OnMouseEnter()
        {
            Publish(this);
        }
    }
}