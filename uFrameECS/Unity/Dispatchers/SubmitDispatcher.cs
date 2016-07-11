using uFrame.Attributes;
using uFrame.ECS.Components;
using UnityEngine.EventSystems;

namespace uFrame.ECS.UnityUtilities
{
    [UFrameEventDispatcher("On Submit"), uFrameCategory("uGUI")]
    public class SubmitDispatcher : EcsDispatcher, ISubmitHandler
    {
        public BaseEventData EventData { get; set; }

        public void OnSubmit(BaseEventData eventData)
        {
            Publish(this);
        }
    }
}