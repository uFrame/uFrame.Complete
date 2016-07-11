using System.Text.RegularExpressions;
using uFrame.Editor.Attributes;
using uFrame.Editor.Graphs.Data;
using uFrame.Editor.Database.Data;
using uFrame.Json;

namespace uFrame.Architect.Editor.Data
{
    public class ShellNodeConfigInput : ShellNodeConfigItem, IShellSlotType, IConnectable
    {
        private bool _allowMultiple;
        private bool _allowSelection;

        public bool IsOutput
        {
            get { return false; }
            set
            {

            }
        }

        [JsonProperty, InspectorProperty]
        public bool AllowMultiple
        {
            get { return _allowMultiple; }
            set
            {
                this.Changed("AllowMultiple", ref _allowMultiple, value);
            }
        }

        public override string TypeName
        {
            get
            {
                return Regex.Replace(Name, @"[^a-zA-Z0-9_\.]+", "");

            }
            set { }
        }

        public override string ClassName
        {
            get { return TypeName; }
        }

        [InspectorProperty, JsonProperty]
        public bool AllowSelection
        {
            get { return _allowSelection; }
            set { this.Changed("AllowSelection", ref _allowSelection, value); }
        }
    }
}