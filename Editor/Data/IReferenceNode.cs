using System.Collections.Generic;
using uFrame.Editor.Configurations;

namespace uFrame.Architect.Editor.Data
{
    public interface IReferenceNode : IShellNode
    {
        string ReferenceClassName { get; }
        [ReferenceSection("Acceptable Types", SectionVisibility.Always, false)]
        IEnumerable<ShellAcceptableReferenceType> AcceptableTypes { get; }
        IEnumerable<IShellNode> PossibleAcceptableTypes { get; }

    }
}