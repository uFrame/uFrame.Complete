using System.Text.RegularExpressions;
using uFrame.Editor.Attributes;
using uFrame.Json;
using uFrame.Editor.Database.Data;

namespace uFrame.Architect.Editor.Data
{
    public class ShellNodeConfigOutput : ShellNodeConfigItem, IShellSlotType
    {
        private bool _allowMultiple;
        private bool _allowSelection;

        public bool IsOutput
        {
            get { return true; }
            set
            {

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
        [JsonProperty, InspectorProperty]
        public bool AllowMultiple
        {
            get { return _allowMultiple; }
            set
            {
                this.Changed("AllowMultiple", ref _allowMultiple, value);
            }
        }

        public bool AllowSelection
        {
            get { return _allowSelection; }
            set { this.Changed("AllowSelection", ref _allowSelection, value); }
        }

        public override string ClassName
        {
            get { return TypeName; }
        }
    }
}