using uFrame.ECS.Editor;
using uFrame.Editor.GraphUI.ViewModels;

namespace uFrame.ECS.Editor
{
    public class ComponentDestroyedNodeViewModel : ComponentDestroyedNodeViewModelBase {
        
        public ComponentDestroyedNodeViewModel(ComponentDestroyedNode graphItemObject, DiagramViewModel diagramViewModel) : 
                base(graphItemObject, diagramViewModel) {
        }
    }
}
