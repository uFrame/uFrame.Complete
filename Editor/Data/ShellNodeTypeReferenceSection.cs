using System.Collections.Generic;
using System.Linq;
using uFrame.Editor.Attributes;
using uFrame.Editor.Configurations;
using uFrame.Json;

namespace uFrame.Architect.Editor.Data
{
    public class ShellNodeTypeReferenceSection : ShellNodeTypeSection, IReferenceNode, IShellConnectable
    {
        [JsonProperty, InspectorProperty]
        public bool IsAutomatic { get; set; }

        [JsonProperty, InspectorProperty]
        public bool AllowDuplicates { get; set; }

        [JsonProperty, InspectorProperty]
        public bool IsEditable { get; set; }

        [JsonProperty, InspectorProperty]
        public bool HasPredefinedOptions { get; set; }

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


        [ReferenceSection("Acceptable Types", SectionVisibility.Always, false)]
        public IEnumerable<ShellAcceptableReferenceType> AcceptableTypes
        {
            get { return PersistedItems.OfType<ShellAcceptableReferenceType>(); }
        }
        public IEnumerable<IShellNode> PossibleAcceptableTypes
        {
            get { return Repository.AllOf<IShellNode>(); }
        }


        [ReferenceSection("Connectable To", SectionVisibility.Always, false)]
        public IEnumerable<ShellConnectableReferenceType> ConnectableTo
        {
            get { return PersistedItems.OfType<ShellConnectableReferenceType>(); }
        }

        public IEnumerable<IShellNode> PossibleConnectableTo
        {
            get { return Repository.AllOf<IShellNode>(); }
        }



        public IEnumerable<IReferenceNode> IncludedInSections
        {
            get
            {
                return Repository.AllOf<IReferenceNode>().Where(p => p.AcceptableTypes.Any(x => x.SourceItem == this));
            }
        }

    }
}