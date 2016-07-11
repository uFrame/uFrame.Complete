using uFrame.Attributes;
using uFrame.ECS.Components;
using UnityEngine.EventSystems;

namespace uFrame.ECS.UnityUtilities
{
    [UFrameEventDispatcher("On Select"), uFrameCategory("uGUI")]
    public class SelectDispatcher : EcsDispatcher, ISelectHandler
    {
        public BaseEventData EventData { get; set; }

        public void OnSelect(BaseEventData eventData)
        {
            EventData = eventData;
            Publish(this);
        }

    }
}