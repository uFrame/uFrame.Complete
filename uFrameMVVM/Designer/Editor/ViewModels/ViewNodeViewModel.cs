using uFrame.Editor.GraphUI.ViewModels;

namespace uFrame.MVVM
{
    public class ViewNodeViewModel : ViewNodeViewModelBase {
        
        public ViewNodeViewModel(ViewNode graphItemObject, DiagramViewModel diagramViewModel) : base(graphItemObject, diagramViewModel)
        {
        }

        public override string IconName
        {
            get { return "ViewIcon"; }
        }
    }
}
