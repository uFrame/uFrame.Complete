using uFrame.Attributes;
using uFrame.ECS.Components;
using UnityEngine.EventSystems;

namespace uFrame.ECS.UnityUtilities
{
    [UFrameEventDispatcher("On Deselect"), uFrameCategory("uGUI")]
    public class DeselectDispatcher : EcsDispatcher, IDeselectHandler
    {
        public BaseEventData EventData { get; set; }

        public void OnDeselect(BaseEventData eventData)
        {
            EventData = eventData;
            Publish(this);
        }

    }
}