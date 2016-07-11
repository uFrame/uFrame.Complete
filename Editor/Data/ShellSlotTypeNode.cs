using System.Collections.Generic;
using System.Linq;
using uFrame.Editor.Attributes;
using uFrame.Editor.Configurations;
using uFrame.Json;

namespace uFrame.Architect.Editor.Data
{
    public class ShellSlotTypeNode : ShellNode, IReferenceNode
    {

        public override string ClassName
        {
            get
            {
                return this.Name;
            }
        }
        [JsonProperty, InspectorProperty]
        public SectionVisibility Visibility { get; set; }

        [ReferenceSection("Acceptable Types", SectionVisibility.Always, false)]
        public IEnumerable<ShellAcceptableReferenceType> AcceptableTypes
        {
            get { return PersistedItems.OfType<ShellAcceptableReferenceType>(); }
        }

        public IEnumerable<IShellNode> PossibleAcceptableTypes
        {
            get { return Repository.AllOf<IShellNode>(); }
        }

        public IShellNode ReferenceType
        {
            get { return AcceptableTypes.Select(p => p.SourceItem).FirstOrDefault(); }
        }

        public IEnumerable<IReferenceNode> IncludedInSections
        {
            get
            {
                return Repository.AllOf<IReferenceNode>().Where(p => p.AcceptableTypes.Any(x => x.SourceItem == this));
            }
        }


        public string ReferenceClassName
        {
            get
            {
                return "I" + Name;
            }
        }

        [InspectorProperty]
        public bool AllowMultiple
        {
            get { return this["Multiple"]; }
            set { this["Multiple"] = value; }
        }

        [InspectorProperty]
        public bool IsOutput
        {
            get { return this["Output"]; }
            set { this["Output"] = value; }
        }
    }
}