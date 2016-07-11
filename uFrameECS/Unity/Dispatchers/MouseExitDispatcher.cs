using uFrame.Attributes;
using uFrame.ECS.Components;

namespace uFrame.ECS.UnityUtilities
{
    [UFrameEventDispatcher("On Mouse Exit"), uFrameCategory("Unity Messages")]
    public class MouseExitDispatcher : EcsDispatcher
    {
        public void OnMouseExit()
        {
            Publish(this);
        }
    }
}