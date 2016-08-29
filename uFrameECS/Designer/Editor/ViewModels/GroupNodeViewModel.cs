using uFrame.ECS.Editor;
using uFrame.Editor.Configurations;
using uFrame.Editor.GraphUI.ViewModels;

namespace uFrame.ECS.Editor
{
    public class GroupNodeViewModel : GroupNodeViewModelBase
    {

        public GroupNodeViewModel(GroupNode graphItemObject, DiagramViewModel diagramViewModel) : 
                base(graphItemObject, diagramViewModel) {
        }


        public virtual bool ShowContextVariables
        {
            get { return IsVisible(SectionVisibility.WhenNodeIsFilter); }
        }
        protected override void CreateContent()
        {
         
            base.CreateContent();

        }
    }
}
