using uFrame.Attributes;
using uFrame.ECS.Components;
using UnityEngine.EventSystems;

namespace uFrame.ECS.UnityUtilities
{
    [UFrameEventDispatcher("On Cancel"), uFrameCategory("uGUI")]
    public class CancelDispatcher : EcsDispatcher, ICancelHandler
    {
        public BaseEventData EventData { get; set; }

        public void OnCancel(BaseEventData eventData)
        {
            Publish(this);
        }
    }
}