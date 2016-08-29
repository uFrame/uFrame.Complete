using uFrame.Editor.GraphUI.ViewModels;

namespace uFrame.MVVM
{


    public class ServiceNodeViewModel : ServiceNodeViewModelBase {
        
        public ServiceNodeViewModel(ServiceNode graphItemObject, DiagramViewModel diagramViewModel) : 
                base(graphItemObject, diagramViewModel) {
        }
    }
}
