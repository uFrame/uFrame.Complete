using uFrame.Editor.Graphs.Data;

namespace uFrame.MVVM
{
    using System;


    public class CollectionsChildItem : CollectionsChildItemBase
    {
        public override bool CanOutputTo(IConnectable input)
        {
            return this.OutputTo<IClassTypeNode>() == null && base.CanOutputTo(input);
        }

        public override string DefaultTypeName
        {
            get { return typeof(int).Name; }
        }
    }

    public partial interface ICollectionsConnectable : IDiagramNodeItem, IConnectable
    {
    }
}
