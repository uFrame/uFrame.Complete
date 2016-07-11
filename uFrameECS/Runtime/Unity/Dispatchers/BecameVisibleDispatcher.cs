using uFrame.Attributes;
using uFrame.ECS.Components;

namespace uFrame.ECS.UnityUtilities
{
    [UFrameEventDispatcher("On Became Visible"), uFrameCategory("Unity Messages")]
    public class BecameVisibleDispatcher : EcsDispatcher
    {
        public void OnBecameVisible()
        {
            Publish(this);
        }
    }
}