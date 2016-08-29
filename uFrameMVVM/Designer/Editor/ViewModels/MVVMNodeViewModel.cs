using uFrame.Editor.GraphUI.ViewModels;

namespace uFrame.MVVM
{


    public class MVVMNodeViewModel : MVVMNodeViewModelBase {
        
        public MVVMNodeViewModel(MVVMNode graphItemObject, DiagramViewModel diagramViewModel) : 
                base(graphItemObject, diagramViewModel) {
        }
    }
}
