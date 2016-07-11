using System.Collections.Generic;
using System.Linq;
using uFrame.Editor.Attributes;
using uFrame.Editor.Configurations;
using uFrame.Editor.Graphs.Data;
using uFrame.Json;

namespace uFrame.Architect.Editor.Data
{
    public class ShellSectionNode : ShellNodeTypeSection, IShellConnectable
    {
        [InputSlot("Reference Type")]
        public ShellSectionReferenceSlot ReferenceSlot { get; set; }

        public IShellNode ReferenceType
        {
            get
            {
                if (ReferenceSlot == null) return null;
                return ReferenceSlot.Item;
            }
        }
        private bool _allowMultipleInputs = true;
        private bool _allowMultipleOutputs = true;

        [JsonProperty, InspectorProperty]
        public bool MultipleInputs
        {
            get { return _allowMultipleInputs; }
            set { _allowMultipleInputs = value; }
        }

        [JsonProperty, InspectorProperty]
        public bool MultipleOutputs
        {
            get { return _allowMultipleOutputs; }
            set { _allowMultipleOutputs = value; }
        }
        public override string ReferenceClassName
        {
            get
            {
                if (ReferenceType == null) return null;
                return ReferenceType.ClassName;
            }
        }

        string IClassTypeNode.ClassName
        {
            get { return ReferenceClassName; }
        }

        public override string ClassName
        {
            get { return ReferenceClassName; }
        }

        [ReferenceSection("Connectable To", SectionVisibility.Always, false)]
        public IEnumerable<ShellConnectableReferenceType> ConnectableTo
        {
            get { return PersistedItems.OfType<ShellConnectableReferenceType>(); }
        }

        public IEnumerable<IShellNode> PossibleConnectableTo
        {
            get { return this.Repository.AllOf<IShellNode>(); }
        }

    }
}