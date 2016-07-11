using uFrame.Editor.GraphUI.ViewModels;

namespace uFrame.MVVM
{


    public class CommandNodeViewModel : CommandNodeViewModelBase {
        
        public CommandNodeViewModel(CommandNode graphItemObject, DiagramViewModel diagramViewModel) : 
                base(graphItemObject, diagramViewModel) {
        }
    }
}
