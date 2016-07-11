using uFrame.Attributes;
using uFrame.ECS.Components;
using UnityEngine.EventSystems;

namespace uFrame.ECS.UnityUtilities
{
    [UFrameEventDispatcher("On Scroll"), uFrameCategory("uGUI")]
    public class ScrollDispatcher : EcsDispatcher, IScrollHandler
    {
        public PointerEventData PointerEventData { get; set; }

        public void OnScroll(PointerEventData eventData)
        {
            PointerEventData = eventData;
            Publish(this);
        }

    }
}