using System.CodeDom;
using UnityEngine;
using uFrame.Editor.Attributes;
using uFrame.Editor.Database.Data;
using uFrame.Editor.Graphs.Data;
using uFrame.Editor.Graphs.Data.Types;
using uFrame.Json;

namespace uFrame.ECS.Editor
{
    public class Vector3Node : Vector3NodeBase {
        private float _x;
        private float _y;
        private float _z;

        [NodeProperty, JsonProperty]
        public float X
        {
            get { return _x; }
            set { this.Changed("X", ref _x, value); }
        }

        [NodeProperty, JsonProperty]
        public float Y
        {
            get { return _y; }
            set { this.Changed("Y", ref _y, value); }
        }

        [NodeProperty, JsonProperty]
        public float Z
        {
            get { return _z; }
            set { this.Changed("Z", ref _z, value); }
        }

        public override string ValueExpression
        {
            get { return string.Format("new Vector3({0}, {1}, {2})", X, Y, Z); }
        }

        public override ITypeInfo VariableType
        {
            get { return new SystemTypeInfo(typeof(Vector3)); }
        }
        public override CodeExpression GetCreateExpression()
        {
            return new CodeSnippetExpression(string.Format("new UnityEngine.Vector3({0}f,{1}f,{2}f)", X, Y, Z));
        }
    }
    
    public partial interface IVector3Connectable : IDiagramNodeItem, IConnectable {
    }
}
