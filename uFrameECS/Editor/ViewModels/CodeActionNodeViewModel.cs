using uFrame.ECS.Editor;
using uFrame.Editor.GraphUI.ViewModels;

namespace uFrame.ECS.Editor
{


    public class CodeActionNodeViewModel : CodeActionNodeViewModelBase {
        
        public CodeActionNodeViewModel(CodeActionNode graphItemObject, DiagramViewModel diagramViewModel) : 
                base(graphItemObject, diagramViewModel) {
        }

        protected override void CreateContent()
        {
            base.CreateContent();
            if (Action.Meta == null)
            {
                ContentItems.Add(new SectionHeaderViewModel()
                {
                    Name = "Please Save And Compile",
                    IsNewLine = true
                });
            }
        }
    }
}
