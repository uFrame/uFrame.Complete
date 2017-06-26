using uFrame.Editor.Core;
using uFrame.Editor.GraphUI.ViewModels;

namespace uFrame.MVVM
{
    public class SetNodeIsStructCommand : Command
    {
        public ISwitchableClassOrStructNodeSystem Item;
        public DiagramNodeViewModel ItemViewModel;
        public bool IsStruct;
    }
}