using uFrame.Kernel;

namespace uFrame.ECS.Components
{
    /// <summary>
    /// Used for dispatching entity events that come from standard unity messages/events.
    /// </summary>
    /// <code>
    ///[UFrameEventDispatcher("On Mouse Down"), uFrameCategory("Unity Messages")]
    ///public class MouseDownDispatcher : EcsDispatcher
    ///{
    ///    public void OnMouseDown()
    ///    {
    ///        Publish(this);
    ///    }
    ///}
    /// </code>
    public class EcsDispatcher : EcsComponent
    {

    }
}