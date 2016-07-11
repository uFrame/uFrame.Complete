using uFrame.Editor.GraphUI.ViewModels;

namespace uFrame.Editor.GraphUI.Drawers
{
    public interface INodeDrawer : IDrawer
    {
        DiagramDrawer Diagram { get; set; }
        //Type CommandsType { get; }
        DiagramNodeViewModel ViewModel { get; set; }
    }
}