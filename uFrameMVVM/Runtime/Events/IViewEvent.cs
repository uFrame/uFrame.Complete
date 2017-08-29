using uFrame.Kernel;
using uFrame.MVVM.Views;

namespace uFrame.MVVM.Events
{
    public interface IViewEvent
    {
        bool IsInstantiated { get; set; }
        IScene Scene { get; set; }
        ViewBase View { get; set; }
    }
}