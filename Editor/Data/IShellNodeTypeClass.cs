using System.Collections.Generic;

namespace uFrame.Architect.Editor.Data
{
    public interface IShellNodeTypeClass : IShellNodeConfigItem
    {
        //bool AllowMultipleInputs { get; set; }
        //bool AllowMultipleOutputs { get; set; }
        bool Inheritable { get; set; }
        IEnumerable<ShellNodeConfigSection> Sections { get; set; }
        IEnumerable<ShellNodeConfigInput> InputSlots { get; set; }
        IEnumerable<ShellNodeConfigOutput> OutputSlots { get; set; }

    }
}