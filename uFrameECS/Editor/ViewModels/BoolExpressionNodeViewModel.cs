using uFrame.ECS.Editor;
using uFrame.Editor.GraphUI.ViewModels;

namespace uFrame.ECS.Editor
{
    public class BoolExpressionNodeViewModel : BoolExpressionNodeViewModelBase {
        
        public BoolExpressionNodeViewModel(BoolExpressionNode graphItemObject, DiagramViewModel diagramViewModel) : 
                base(graphItemObject, diagramViewModel) {
        }
    }
}
