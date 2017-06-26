using System.Collections.Generic;
using uFrame.Editor.GraphUI.ViewModels;

namespace uFrame.MVVM
{


    public class SimpleClassNodeViewModel : SimpleClassNodeViewModelBase {

        public SimpleClassNodeViewModel(SimpleClassNode graphItemObject, DiagramViewModel diagramViewModel) :
                base(graphItemObject, diagramViewModel) {
        }

        public override IEnumerable<string> Tags
        {
            get
            {
                if (((ISwitchableClassOrStructNodeSystem) GraphItemObject).IsStruct)
                    yield return "Struct";
            }
        }
    }
}
