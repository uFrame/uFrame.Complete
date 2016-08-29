using uFrame.ECS.Editor;
using uFrame.Editor.Configurations;
using uFrame.Editor.GraphUI.ViewModels;

namespace uFrame.ECS.Editor
{


    public class PropertyChangedNodeViewModel : PropertyChangedNodeViewModelBase {
        
        public PropertyChangedNodeViewModel(PropertyChangedNode graphItemObject, DiagramViewModel diagramViewModel) : 
                base(graphItemObject, diagramViewModel) {
        }

        public PropertyChangedNode PropertyChangedNode
        {
            get { return GraphItemObject as PropertyChangedNode; }
        }

        public override bool AutoAddProperties
        {
            get { return false; }
        }

        protected override void CreateContent()
        {
            base.CreateContent();
            //if (PropertyChangedNode.EntityGroup.Item != null)
            //{
            if (IsVisible(SectionVisibility.WhenNodeIsNotFilter))
            {
                var propertySelection = new InputOutputViewModel()
                {
                    DataObject = PropertyChangedNode.PropertyIn,
                    Name = "Property",
                    IsInput = true,
                    IsOutput = false,
                    IsNewLine = true,
                    AllowSelection = true
                };
                ContentItems.Add(propertySelection);
                AddPropertyFields();
            }
              
            //}
           
        
        }



        public PropertyChangedNode ChangedNode
        {
            get { return GraphItem as PropertyChangedNode; }
        }

    }
}
