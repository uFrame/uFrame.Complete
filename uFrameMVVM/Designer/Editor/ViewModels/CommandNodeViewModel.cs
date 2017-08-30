using System.Collections.Generic;
using uFrame.Editor.GraphUI.ViewModels;
using uFrame.Json;

namespace uFrame.MVVM
{
    public class CommandNodeViewModel : CommandNodeViewModelBase {
        public CommandNodeViewModel(CommandNode graphItemObject, DiagramViewModel diagramViewModel) :
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
