using uFrame.Editor.GraphUI.ViewModels;

namespace uFrame.MVVM
{


    public class SubSystemNodeViewModel : SubSystemNodeViewModelBase {
        
        public SubSystemNodeViewModel(SubSystemNode graphItemObject, DiagramViewModel diagramViewModel) : 
                base(graphItemObject, diagramViewModel) {
        }

        public override string IconName
        {
            get { return "SubsystemIcon"; }
        }
    }
}
