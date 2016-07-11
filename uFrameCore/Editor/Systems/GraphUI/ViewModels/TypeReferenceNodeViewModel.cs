using System.Collections.Generic;
using uFrame.Editor.Compiling.CommonNodes;
using uFrame.Editor.GraphUI.Drawers.Schemas;
using uFrame.Editor.Platform;

namespace uFrame.Editor.GraphUI.ViewModels
{
    public class TypeReferenceNodeViewModel : DiagramNodeViewModel<TypeReferenceNode>
    {
        public TypeReferenceNodeViewModel(TypeReferenceNode graphItemObject, DiagramViewModel diagramViewModel) : base(graphItemObject, diagramViewModel)
        {
        }

        public override INodeStyleSchema StyleSchema
        {
            get { return CachedStyles.NodeStyleSchemaMinimalistic; }
        }

        public override IEnumerable<string> Tags
        {
            get { yield return "Type Reference"; }
        }

        //public override bool IsEditable
        //{
        //    get { return false; }
        //}

        public override void DataObjectChanged()
        {
            base.DataObjectChanged();

        }
    }
}