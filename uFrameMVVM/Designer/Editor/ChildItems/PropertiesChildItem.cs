using uFrame.Editor.Graphs.Data;

namespace uFrame.MVVM
{
    using System;


    public class PropertiesChildItem : PropertiesChildItemBase
    {
        public override bool AllowInputs
        {
            get { return false; }
        }

        public override bool CanOutputTo(IConnectable input)
        {
            if (input is ComputedPropertyNode) return true;
            if (this.OutputTo<IClassTypeNode>() != null)
            {
                return false;
            }
            return base.CanOutputTo(input);
        }

        public override string DefaultTypeName
        {
            get { return typeof(int).Name; }
        }
    }

    public partial interface IPropertiesConnectable : IDiagramNodeItem, IConnectable
    {
    }
}
