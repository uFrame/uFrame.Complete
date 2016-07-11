using uFrame.Attributes;
using uFrame.ECS.Components;
using UnityEngine.EventSystems;

namespace uFrame.ECS.UnityUtilities
{
    [UFrameEventDispatcher("Pointer Exit"), uFrameCategory("uGUI")]
    public class PointerExitDispatcher : EcsDispatcher, IPointerExitHandler
    {
        public PointerEventData PointerEventData { get; set; }
        public void OnPointerExit(PointerEventData eventData)
        {
            PointerEventData = eventData;
            Publish(this);
        }
    }
}