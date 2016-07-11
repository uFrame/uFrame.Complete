using uFrame.ECS.Editor;
using uFrame.Editor.GraphUI.ViewModels;

namespace uFrame.ECS.Editor
{
    public class Vector3NodeViewModel : Vector3NodeViewModelBase {
        
        public Vector3NodeViewModel(Vector3Node graphItemObject, DiagramViewModel diagramViewModel) : 
                base(graphItemObject, diagramViewModel) {
        }
    }
}
