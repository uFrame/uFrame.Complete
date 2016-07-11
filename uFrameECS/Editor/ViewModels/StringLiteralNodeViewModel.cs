using uFrame.ECS.Editor;
using uFrame.Editor.GraphUI.ViewModels;

namespace uFrame.ECS.Editor
{ 
    public class StringLiteralNodeViewModel : StringLiteralNodeViewModelBase {
        
        public StringLiteralNodeViewModel(StringLiteralNode graphItemObject, DiagramViewModel diagramViewModel) : 
                base(graphItemObject, diagramViewModel) {
        }
    }
}
