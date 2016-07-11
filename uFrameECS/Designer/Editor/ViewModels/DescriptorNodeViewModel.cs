using uFrame.ECS.Editor;
using uFrame.Editor.GraphUI.ViewModels;

namespace uFrame.ECS.Editor
{ 
    public class DescriptorNodeViewModel : DescriptorNodeViewModelBase {
        
        public DescriptorNodeViewModel(DescriptorNode graphItemObject, DiagramViewModel diagramViewModel) : 
                base(graphItemObject, diagramViewModel) {
        }
    }
}
