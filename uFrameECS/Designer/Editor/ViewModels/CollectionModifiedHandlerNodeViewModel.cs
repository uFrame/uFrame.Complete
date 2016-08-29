using uFrame.ECS.Editor;
using uFrame.Editor.Configurations;
using uFrame.Editor.GraphUI.ViewModels;

namespace uFrame.ECS.Editor
{


    public class CollectionModifiedHandlerNodeViewModel : CollectionModifiedHandlerNodeViewModelBase {
        
        public CollectionModifiedHandlerNodeViewModel(CollectionModifiedHandlerNode graphItemObject, DiagramViewModel diagramViewModel) : 
                base(graphItemObject, diagramViewModel) {
        }

        public override bool AutoAddProperties
        {
            get { return false; }
        }

        protected override void CreateContent()
        {
            base.CreateContent();
            if (IsVisible(SectionVisibility.WhenNodeIsNotFilter))
            {
                var propertySelection = new InputOutputViewModel()
                {
                    DataObject = PropertyChangedNode.CollectionIn,
                    Name = "Collection",
                    IsInput = true,
                    IsOutput = false,
                    IsNewLine = true,
                    AllowSelection = true
                };
                ContentItems.Add(propertySelection);
                AddPropertyFields();
            }
           
        }

        public CollectionModifiedHandlerNode PropertyChangedNode
        {
            get { return GraphItemObject as CollectionModifiedHandlerNode; }
        }
    }
}
