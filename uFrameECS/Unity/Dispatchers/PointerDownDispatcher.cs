using uFrame.Attributes;
using uFrame.ECS.Components;
using UnityEngine.EventSystems;

namespace uFrame.ECS.UnityUtilities
{
    [UFrameEventDispatcher("Pointer Down"), uFrameCategory("uGUI")]
    public class PointerDownDispatcher : EcsDispatcher, IPointerDownHandler
    {
        public PointerEventData PointerEventData { get; set; }
        public void OnPointerDown(PointerEventData eventData)
        {
            PointerEventData = eventData;
            Publish(this);
        }
    }
}