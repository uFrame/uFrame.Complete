using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using uFrame.Editor.Attributes;
using uFrame.Editor.Configurations;
using uFrame.Editor.Graphs.Data;
using uFrame.Json;

namespace uFrame.Architect.Editor.Data
{
    public class ShellNodeTypeNode : ShellInheritableNode, IShellNode, IShellConnectable
    {
        private string _classFormat = "{0}";
        private bool _allowMultipleOutputs;

        [JsonProperty, InspectorProperty]
        public bool MultipleInputs { get; set; }

        [JsonProperty, InspectorProperty]
        public bool MultipleOutputs
        {
            get
            {
                if (this["Inheritable"])
                {
                    return true;
                }
                return _allowMultipleOutputs;
            }
            set { _allowMultipleOutputs = value; }
        }

        public NodeColor Color
        {
            get
            {

                return NodeColor.Gray;
            }
        }

        [Browsable(false)]
        public IShellNode ReferenceType
        {
            get { return GetConnectionReference<ReferenceItemType>().InputFrom<IShellNode>(); }
        }
        [InspectorProperty]
        public bool Inheritable
        {
            get
            {
                return this["Inheritable"];
            }
            set { this["Inheritable"] = value; }

        }


        [Browsable(false)]
        [OutputSlot("Sub Nodes")]
        public MultiOutputSlot<ShellNodeTypeNode> SubNodesSlot { get; set; }
        [Browsable(false)]
        public IEnumerable<ShellNodeTypeNode> SubNodes
        {
            get { return SubNodesSlot.Items; }
        }


        [Browsable(false)]
        [ReferenceSection("Sections", SectionVisibility.WhenNodeIsFilter, false)]
        public IEnumerable<ShellNodeSectionsSlot> Sections
        {
            get { return PersistedItems.OfType<ShellNodeSectionsSlot>(); }
        }
        [Browsable(false)]
        public IEnumerable<ShellNodeTypeSection> PossibleSections
        {
            get { return this.Repository.AllOf<ShellNodeTypeSection>(); }
        }
        [Browsable(false)]
        public IEnumerable<IReferenceNode> IncludedInSections
        {
            get
            {
                return this.Repository.AllOf<IReferenceNode>().Where(p => p.AcceptableTypes.Any(x => x.SourceItem == this));
            }
        }
        [Browsable(false)]
        public IEnumerable<ShellNodeTypeReferenceSection> ReferenceSections
        {
            get { return Sections.Select(p => p.SourceItem).OfType<ShellNodeTypeReferenceSection>(); }
        }
        [Browsable(false)]
        [ReferenceSection("Inputs", SectionVisibility.WhenNodeIsFilter, true)]
        public IEnumerable<ShellNodeInputsSlot> InputSlots
        {
            get
            {
                return PersistedItems.OfType<ShellNodeInputsSlot>();
            }
        }
        [Browsable(false)]
        [ReferenceSection("Outputs", SectionVisibility.WhenNodeIsFilter, true)]
        public IEnumerable<ShellNodeOutputsSlot> OutputSlots
        {
            get
            {
                return PersistedItems.OfType<ShellNodeOutputsSlot>();
            }
        }
        [Browsable(false)]
        public IEnumerable<ShellSlotTypeNode> PossibleInputSlots
        {
            get { return this.Repository.AllOf<ShellSlotTypeNode>().Where(p => !p.IsOutput); }
        }
        [Browsable(false)]
        public IEnumerable<ShellSlotTypeNode> PossibleOutputSlots
        {
            get { return this.Repository.AllOf<ShellSlotTypeNode>().Where(p => p.IsOutput); }
        }

        //[Section("Custom Selectors", SectionVisibility.WhenNodeIsFilter)]
        [Browsable(false)]
        public IEnumerable<ShellPropertySelectorItem> CustomSelectors
        {
            get
            {
                return PersistedItems.OfType<ShellPropertySelectorItem>();
            }
        }

        public override string ClassName
        {
            get { return Name + "Node"; }
        }
        [Browsable(false)]
        [ReferenceSection("Connectable To", SectionVisibility.WhenNodeIsFilter, false)]
        public IEnumerable<ShellConnectableReferenceType> ConnectableTo
        {
            get { return PersistedItems.OfType<ShellConnectableReferenceType>(); }
        }

        [Browsable(false)]
        public IEnumerable<IShellNode> PossibleConnectableTo
        {
            get { return this.Repository.AllOf<IShellNode>(); }
        }


    }
}