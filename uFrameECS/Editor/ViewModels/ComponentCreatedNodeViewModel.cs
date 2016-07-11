using uFrame.ECS.Editor;
using uFrame.Editor.GraphUI.ViewModels;

namespace uFrame.ECS.Editor
{
    public class ComponentCreatedNodeViewModel : ComponentCreatedNodeViewModelBase {
        
        public ComponentCreatedNodeViewModel(ComponentCreatedNode graphItemObject, DiagramViewModel diagramViewModel) : 
                base(graphItemObject, diagramViewModel) {
        }
    }
}
