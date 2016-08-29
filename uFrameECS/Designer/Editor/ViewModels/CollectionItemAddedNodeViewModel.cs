using uFrame.ECS.Editor;
using uFrame.Editor.GraphUI.ViewModels;

namespace uFrame.ECS.Editor
{
    public class CollectionItemAddedNodeViewModel : CollectionItemAddedNodeViewModelBase {
        
        public CollectionItemAddedNodeViewModel(CollectionItemAddedNode graphItemObject, DiagramViewModel diagramViewModel) : 
                base(graphItemObject, diagramViewModel) {
        }
    }
}
