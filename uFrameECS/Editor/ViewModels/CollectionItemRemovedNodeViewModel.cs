using uFrame.ECS.Editor;
using uFrame.Editor.GraphUI.ViewModels;

namespace uFrame.ECS.Editor
{
    public class CollectionItemRemovedNodeViewModel : CollectionItemRemovedNodeViewModelBase {
        
        public CollectionItemRemovedNodeViewModel(CollectionItemRemovedNode graphItemObject, DiagramViewModel diagramViewModel) : 
                base(graphItemObject, diagramViewModel) {
        }
    }
}
