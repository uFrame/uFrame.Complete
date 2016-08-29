using uFrame.Attributes;
using uFrame.ECS.Components;
using UnityEngine.EventSystems;

namespace uFrame.ECS.UnityUtilities
{
    [UFrameEventDispatcher("On Drop"), uFrameCategory("uGUI")]
    public class DropDispatcher : EcsDispatcher, IDropHandler
    {
        public PointerEventData PointerEventData { get; set; }

        public void OnDrop(PointerEventData eventData)
        {
            PointerEventData = eventData;
            Publish(this);
        }
    }
}