using uFrame.Editor.Configurations;
using uFrame.Editor.Graphs.Data;
using uFrame.Editor.GraphUI.ViewModels;

namespace uFrame.ECS.Editor
{
    using System.Collections.Generic;
    using System.Linq;


    public class SequenceItemNodeViewModel : SequenceItemNodeViewModelBase
    {

        public SequenceItemNodeViewModel(SequenceItemNode graphItemObject, DiagramViewModel diagramViewModel) :
                base(graphItemObject, diagramViewModel)
        {
        }

        public SequenceItemNode SequenceNode
        {
            get { return GraphItem as SequenceItemNode; }
        }


        public override void DataObjectChanged()
        {
            base.DataObjectChanged();
            IsBreakpoint = SequenceNode.BreakPoint != null;
        }
        public bool IsBreakpoint { get; set; }


        public virtual string SecondTitle
        {
            get { return SequenceNode.SecondTitle; }
        }


        public override IEnumerable<string> Tags
        {
            get
            {
                if (!string.IsNullOrEmpty(SecondTitle)) yield return Name;
                var sequenceContainer = SequenceNode.Graph.CurrentFilter as ISequenceNode;
                if (sequenceContainer != null && sequenceContainer.StartNode == SequenceNode)
                {
                    yield return "Start";
                }
                foreach (var item in SequenceNode.Flags)
                {
                    yield return item.Name;
                }

                yield break;
            }
        }

        protected override void CreateContent()
        {
            InputConnectorType = NodeConfig.SourceType;
            OutputConnectorType = NodeConfig.SourceType;
            if (AutoAddProperties)
                AddPropertyFields();

            CreateActionContent();
        }

        protected virtual void CreateActionContent()
        {
            CreateContentByConfiguration(NodeConfig.GraphItemConfigurations, GraphItem);

            CreateActionIns();
            CreateActionOuts();
        }

        protected virtual void CreateActionOuts()
        {
            foreach (var item in SequenceNode.GraphItems.OfType<IActionOut>())
            {
                var vm = new InputOutputViewModel()
                {
                    Name = item.Name,
                    DataObject = item,
                    IsOutput = true,
                    IsNewLine =
                        item.ActionFieldInfo == null || item.ActionFieldInfo.DisplayType == null
                            ? true
                            : item.ActionFieldInfo.DisplayType.IsNewLine,
                    DiagramViewModel = DiagramViewModel
                };
                ContentItems.Add(vm);

	            if (!(item is ActionBranch))
                {
                    vm.OutputConnector.Style = ConnectorStyle.Circle;
                    vm.OutputConnector.TintColor = UnityEngine.Color.green;
                }
            }
        }

        protected virtual void CreateActionIns()
        {
            foreach (var item in SequenceNode.GraphItems.OfType<IActionIn>())
            {
                var vm = new InputOutputViewModel()
                {
                    Name = item.Name,
                    IsOutput = false,
                    IsInput = true,
                    DataObject = item,
                    IsNewLine =
                        item.ActionFieldInfo == null || item.ActionFieldInfo.DisplayType == null
                            ? true
                            : item.ActionFieldInfo.DisplayType.IsNewLine,
                    DiagramViewModel = DiagramViewModel
                };
                ContentItems.Add(vm);
                if (vm.InputConnector != null && !(item is BranchesChildItem))
                {
                    vm.InputConnector.Style = ConnectorStyle.Circle;
                    vm.InputConnector.TintColor = UnityEngine.Color.green;
                }
            }
        }

        public virtual bool AutoAddProperties
        {
            get { return true; }
        }
    }
}
