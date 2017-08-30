using uFrame.Kernel;
using uFrame.MVVM.Views;

namespace uFrame.MVVM.Events
{
    public struct ViewDestroyedEvent : IViewEvent
    {
        public bool IsInstantiated { get; set; }
        public IScene Scene { get; set; }
        public ViewBase View { get; set; }
    }
}