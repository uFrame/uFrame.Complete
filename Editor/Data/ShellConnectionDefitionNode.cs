using uFrame.Editor.Configurations;
using uFrame.Editor.Graphs.Data;

namespace uFrame.Architect.Editor.Data
{
    public class ShellConnectionDefitionNode
    {
        [InputSlot("Output")]
        public SingleInputSlot<IOutputCapable> OutputItem { get; set; }
        [OutputSlot("Input")]
        public SingleInputSlot<IInputCapable> InputItem { get; set; }
    }
}