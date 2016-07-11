using System.Linq;
using uFrame.Editor.Core;
using uFrame.Editor.Graphs.Data;
using uFrame.Editor.GraphUI.Drawers.Schemas;
using uFrame.Editor.GraphUI.ViewModels;
using uFrame.Editor.Platform;

namespace uFrame.Architect.Editor.Data
{
    public class ShellNodeConfigViewModel : GenericNodeViewModel<ShellNodeConfig>
    {
        public ShellNodeConfigViewModel(ShellNodeConfig graphItemObject, DiagramViewModel diagramViewModel)
            : base(graphItemObject, diagramViewModel)
        {
        }

        public override INodeStyleSchema StyleSchema
        {
            get
            {
                switch (GraphItem.NodeStyle)
                {
                    case NodeStyle.Normal:
                        return NormalStyleSchema;
                    case NodeStyle.Minimalistic:
                        return MinimalisticStyleSchema;
                    case NodeStyle.Bold:
                        return BoldStyleSchema;
                }
                return base.StyleSchema;
            }
        }

        public override NodeColor Color
        {
            get { return GraphItem.Color; }
        }

        public override bool IsCollapsed
        {
            get { return false; }
            set { base.IsCollapsed = value; }
        }

        public override bool AllowCollapsing
        {
            get { return false; }
        }

        protected override void CreateContent()
        {
            //base.CreateContent();

            foreach (var column in GraphItem.ChildItemsWithInherited.OfType<IShellNodeConfigItem>().GroupBy(p => p.Column))
            {
                foreach (var item in column.OrderBy(p => p.Row))
                {
                    if (!IsVisible(item.Visibility)) continue;
                    var section = item as ShellNodeConfigSection;
                    if (section != null)
                    {
                        CreateHeader(section, section);
                        continue;
                    }
                    var sectionPointer = item as ShellNodeConfigSectionPointer;
                    if (sectionPointer != null)
                    {
                        CreateHeader(sectionPointer.SourceItem, sectionPointer);
                        continue;
                    }

                    var input = item as ShellNodeConfigInput;
                    if (input != null)
                    {
                        CreateInput(input, input);
                        continue;
                    }
                    var inputPointer = item as ShellNodeConfigInputPointer;
                    if (inputPointer != null)
                    {
                        CreateInput(inputPointer.SourceItem, inputPointer);
                        continue;
                    }

                    var output = item as ShellNodeConfigOutput;
                    if (output != null)
                    {
                        CreateOutput(output, output);
                        continue;
                    }
                    var outputPointer = item as ShellNodeConfigOutputPointer;
                    if (outputPointer != null)
                    {
                        CreateOutput(outputPointer.SourceItem, outputPointer);
                        continue;
                    }
                }
            }
        }

        private void CreateSelector(ShellNodeConfigSelector input)
        {

        }

        private void CreateOutput(ShellNodeConfigOutput output, object dataObject)
        {
            var vm = new InputOutputViewModel()
            {
                IsInput = false,
                IsOutput = true,
                DiagramViewModel = this.DiagramViewModel,
                Name = output.Name,
                DataObject = dataObject,
                Column = output.Column,
                ColumnSpan = output.ColumnSpan,
                IsNewLine = output.IsNewRow
            };
            ContentItems.Add(vm);
        }

        private void CreateInput(ShellNodeConfigInput input, object dataObject)
        {
            var vm = new InputOutputViewModel()
            {
                IsInput = true,
                IsOutput = false,
                DiagramViewModel = this.DiagramViewModel,
                Name = input.Name,
                DataObject = dataObject,
                Column = input.Column,
                ColumnSpan = input.ColumnSpan,
                IsNewLine = input.IsNewRow,
                AllowSelection = input.AllowSelection
            };
            ContentItems.Add(vm);
        }

        private void CreateHeader(ShellNodeConfigSection item, object dataObject)
        {
            var sectionViewModel = new GenericItemHeaderViewModel()
            {
                Name = item.Name,
                AddCommand = item.AllowAdding ? new LambdaCommand("", () => { }) : null,
                DataObject = dataObject,
                NodeViewModel = this,
                AllowConnections = true,
                Column = item.Column,
                ColumnSpan = item.ColumnSpan,
                IsNewLine = item.IsNewRow
            };
            ContentItems.Add(sectionViewModel);
        }

        public void AddSectionItem()
        {
            DiagramViewModel.CurrentRepository.Add(new ShellNodeConfigSection()
            {
                Node = GraphItem,
                Name = "New Section",
                IsNewRow = true,
            });
        }

        public void AddInputItem()
        {
            DiagramViewModel.CurrentRepository.Add(new ShellNodeConfigInput()
            {
                Node = GraphItem,
                Name = "New Input",
                IsNewRow = true,

            });

        }

        public void AddOutputItem()
        {
            DiagramViewModel.CurrentRepository.Add(new ShellNodeConfigOutput()
            {
                Node = GraphItem,
                Name = "New Output",
                IsNewRow = true,

            });
        }

        public void RemoveSelected()
        {
            DiagramViewModel.CurrentRepository.Remove(ContentItems.First(p => p.IsSelected).DataObject as IDiagramNodeItem);
        }

        public void AddSectionPointer(ShellNodeConfigSection item)
        {
            DiagramViewModel.CurrentRepository.Add(new ShellNodeConfigSectionPointer()
            {
                Node = GraphItem,
                SourceIdentifier = item.Identifier

            });
        }
        public void AddInputPointer(ShellNodeConfigInput item)
        {
            DiagramViewModel.CurrentRepository.Add(new ShellNodeConfigInputPointer()
            {
                Node = GraphItem,
                SourceIdentifier = item.Identifier
            });
        }
        public void AddOutputPointer(ShellNodeConfigOutput item)
        {
            DiagramViewModel.CurrentRepository.Add(new ShellNodeConfigOutputPointer()
            {
                Node = GraphItem,
                SourceIdentifier = item.Identifier
            });
        }
    }
}