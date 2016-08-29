using uFrame.ECS.Editor;
using uFrame.Editor.Graphs.Data;
using uFrame.Editor.GraphUI.ViewModels;

namespace uFrame.ECS.Editor
{
    using System.Linq;
    public class VariableNodeViewModel : VariableNodeViewModelBase {
        
        public VariableNodeViewModel(VariableNode graphItemObject, DiagramViewModel diagramViewModel) : 
                base(graphItemObject, diagramViewModel) {
        }
        public VariableNode Variable
        {
            get
            {
                return GraphItem as VariableNode;
            }
        }
        public override string Name
        {
            get
            {
                return !string.IsNullOrEmpty(Variable.VariableName) ?
                    Variable.VariableName : base.Name;
            }
        }
        protected override void CreateContent()
        {
            
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
           
            base.CreateContent();
        }
    }
}
