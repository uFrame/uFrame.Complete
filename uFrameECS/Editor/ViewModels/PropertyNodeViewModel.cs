using uFrame.ECS.Editor;
using uFrame.Editor.Graphs.Data;
using uFrame.Editor.GraphUI.ViewModels;

namespace uFrame.ECS.Editor
{
    using System.Collections.Generic;
    using System.Linq;


    public class PropertyNodeViewModel : PropertyNodeViewModelBase {
        private string _name;

        public PropertyNodeViewModel(PropertyNode graphItemObject, DiagramViewModel diagramViewModel) : 
                base(graphItemObject, diagramViewModel) {
        }

        public PropertyNode PropertyNode
        {
            get
            {
                return DataObject as PropertyNode;
            }
        }
        public override IEnumerable<string> Tags
        {
            get
            {
                if (PropertyNode.VariableType != null)
                {
                    yield return PropertyNode.VariableType.TypeName;
                }
                yield break;
            }
        }

        public override void DataObjectChanged()
        {
            base.DataObjectChanged();
            _name =  GraphItem.Title;
        }

        public override string Name
        {
            get { return _name; }
            set { base.Name = value; }
        }
        
        protected override void CreateContent()
        {
            //base.CreateContent();
            foreach (var item in GraphItem.GraphItems.OfType<GenericSlot>())
            {
                var vm = new InputOutputViewModel()
                {
                    Name = item.Name,
                    IsOutput = item is IActionOut,
                    IsInput = !(item is IActionOut),
                    DataObject = item,
                    IsNewLine = true,
                    DiagramViewModel = DiagramViewModel
                };
                ContentItems.Add(vm);
                if (vm.InputConnector != null)
                {
                    vm.InputConnector.Style = ConnectorStyle.Circle;
                    vm.InputConnector.TintColor = UnityEngine.Color.green;
                }

            }
           
        }
    }
}
