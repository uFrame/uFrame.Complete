using uFrame.ECS.Editor;
using uFrame.Editor.Configurations;
using uFrame.Editor.Graphs.Data;
using uFrame.Editor.GraphUI.ViewModels;

namespace uFrame.ECS.Editor
{
    public class ComponentNodeViewModel : ComponentNodeViewModelBase {
        
        public ComponentNodeViewModel(ComponentNode graphItemObject, DiagramViewModel diagramViewModel) : 
                base(graphItemObject, diagramViewModel) {
        }


        public override string IconName
        {
            get
            {
                return ComponentNode != null && !string.IsNullOrEmpty(ComponentNode.CustomIcon)
                    ? ComponentNode.CustomIcon
                    : base.IconName;
            }
        }

        public override string SubTitle
        {
            get
            {
                if (ComponentNode.BlackBoard)
                {
                    return "Black Board Component";
                }
                return base.SubTitle;
            }
        }

        public ComponentNode ComponentNode
        {
            get
            {
                return DataObject as ComponentNode;
            }
        }

        protected override void OnAdd(NodeConfigSectionBase configSection, GenericNodeChildItem item)
        {
            base.OnAdd(configSection, item);
            
        }

        public override NodeColor Color
        {
            get
            {
                if (ComponentNode.BlackBoard)
                {
                    return NodeColor.Black;
                }
                return base.Color;
            }
        }
    }
}
