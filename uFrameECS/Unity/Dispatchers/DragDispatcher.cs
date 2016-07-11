using uFrame.Attributes;
using uFrame.ECS.Components;
using UnityEngine.EventSystems;

namespace uFrame.ECS.UnityUtilities
{
    [UFrameEventDispatcher("On Drag"), uFrameCategory("uGUI")]
    public class DragDispatcher : EcsDispatcher, IDragHandler
    {
        public PointerEventData PointerEventData { get; set; }

        public void OnDrag(PointerEventData eventData)
        {
            PointerEventData = eventData;
            Publish(this);
        }
    }
}