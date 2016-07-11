using uFrame.Editor.Graphs.Data;

namespace uFrame.Architect.Editor.Data
{
    public class ShellSectionReferenceSlot : SingleInputSlot<ShellChildItemTypeNode>
    {
        public override bool Validate(IDiagramNodeItem a, IDiagramNodeItem b)
        {
            return true;
            return base.Validate(a, b);
        }

        public override bool ValidateInput(IDiagramNodeItem arg1, IDiagramNodeItem arg2)
        {
            return true;
            return base.ValidateInput(arg1, arg2);
        }

        public override bool ValidateOutput(IDiagramNodeItem arg1, IDiagramNodeItem arg2)
        {
            return true;
            return base.ValidateOutput(arg1, arg2);
        }
    }
}