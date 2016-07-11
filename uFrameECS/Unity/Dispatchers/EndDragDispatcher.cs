using uFrame.Attributes;
using uFrame.ECS.Components;
using UnityEngine.EventSystems;

namespace uFrame.ECS.UnityUtilities
{
    [UFrameEventDispatcher("On End Drag"), uFrameCategory("uGUI")]
    public class EndDragDispatcher : EcsDispatcher, IEndDragHandler
    {
        public PointerEventData PointerEventData { get; set; }

        public void OnEndDrag(PointerEventData eventData)
        {
            PointerEventData = eventData;
            Publish(this);
        }
    }
}