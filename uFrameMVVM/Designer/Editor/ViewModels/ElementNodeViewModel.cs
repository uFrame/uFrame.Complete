using uFrame.Editor.GraphUI.ViewModels;

namespace uFrame.MVVM
{


    public class ElementNodeViewModel : ElementNodeViewModelBase
    {

        public ElementNodeViewModel(ElementNode graphItemObject, DiagramViewModel diagramViewModel) :
                base(graphItemObject, diagramViewModel)
        {
        }

        public override string IconName
        {
            get { return "ElementIcon"; }
        }
    }
}
