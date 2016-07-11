using uFrame.ECS.Editor;
using uFrame.Editor.GraphUI.ViewModels;

namespace uFrame.ECS.Editor
{
    public class NullNodeViewModel : NullNodeViewModelBase {
        
        public NullNodeViewModel(NullNode graphItemObject, DiagramViewModel diagramViewModel) : 
                base(graphItemObject, diagramViewModel) {
        }
    }
}
