using uFrame.ECS.Editor;
using uFrame.Editor.Configurations;
using uFrame.Editor.GraphUI.ViewModels;

namespace uFrame.ECS.Editor
{
    using System.Collections.Generic;


    public class CustomActionNodeViewModel : CustomActionNodeViewModelBase {
        
        public CustomActionNodeViewModel(CustomActionNode graphItemObject, DiagramViewModel diagramViewModel) : 
                base(graphItemObject, diagramViewModel) {
        }

        protected override void CreateContent()
        {
            base.CreateContent();
      
        }

        public override IEnumerable<string> Tags
        {
            get
            {
                if (CustomActionNode.CodeAction)
                {
                    yield return "Code Action";
                }
            }
        }

        public CustomActionNode CustomActionNode
        {
            get
            {
                return DataObject as CustomActionNode;
            }
        }


        protected override void CreateActionContent()
        {
            if (IsVisible(SectionVisibility.WhenNodeIsFilter))
            {
                CreateActionIns();
                CreateActionOuts();
            }
            else
            {
                CreateContentByConfiguration(NodeConfig.GraphItemConfigurations, GraphItem);
            }
        }
    }
}
