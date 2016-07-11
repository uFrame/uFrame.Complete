using uFrame.Attributes;
using uFrame.ECS.Components;

namespace uFrame.ECS.UnityUtilities
{
    [UFrameEventDispatcher("On Became Invisible"), uFrameCategory("Unity Messages")]
    public class BecameInvisibleDispatcher : EcsDispatcher
    {
        public void OnBecameInvisible()
        {
            Publish(this);
        }
    }
}