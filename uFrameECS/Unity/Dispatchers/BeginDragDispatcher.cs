using uFrame.Attributes;
using uFrame.ECS.Components;
using UnityEngine.EventSystems;

namespace uFrame.ECS.UnityUtilities
{
    [UFrameEventDispatcher("On Begin Drag"), uFrameCategory("uGUI")]
    public class BeginDragDispatcher : EcsDispatcher, IBeginDragHandler
    {
        public PointerEventData PointerEventData { get; set; }

        public void OnBeginDrag(PointerEventData eventData)
        {
            PointerEventData = eventData;
            Publish(this);
        }
    }
}