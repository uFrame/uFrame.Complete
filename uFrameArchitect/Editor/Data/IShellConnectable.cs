using System.Collections.Generic;
using uFrame.Editor.Configurations;
using uFrame.Editor.Graphs.Data;

namespace uFrame.Architect.Editor.Data
{
    public interface IShellConnectable : IDiagramNode, IShellNode
    {
        [ReferenceSection("Connectable To", SectionVisibility.Always, false)]
        IEnumerable<ShellConnectableReferenceType> ConnectableTo { get; }

        bool MultipleInputs { get; set; }

        bool MultipleOutputs { get; set; }
    }
}