using System.Collections.Generic;
using System.Text.RegularExpressions;
using uFrame.Editor.Attributes;
using uFrame.Editor.Configurations;
using uFrame.Editor.Graphs.Data;
using uFrame.Editor.Database.Data;
using uFrame.Json;

namespace uFrame.Architect.Editor.Data
{
    public class ShellNodeConfigItem : GenericNodeChildItem, IShellNodeConfigItem, IClassTypeNode
    {
        [JsonProperty, InspectorProperty]
        public int Row
        {
            get { return _row; }
            set { this.Changed("Row", ref _row, value); }
        }

        [JsonProperty, InspectorProperty]
        public int Column
        {
            get { return _column; }
            set { this.Changed("Column", ref _column, value); }
        }

        [JsonProperty, InspectorProperty]
        public int ColumnSpan
        {
            get { return _columnSpan; }
            set { this.Changed("ColumnSpan", ref _columnSpan, value); }
        }

        [JsonProperty, InspectorProperty]
        public bool IsNewRow
        {
            get { return _isNewRow; }
            set { this.Changed("IsNewRow", ref _isNewRow, value); }
        }

        [JsonProperty, InspectorProperty(InspectorType.TextArea)]
        public string Comments
        {
            get { return _comments; }
            set { this.Changed("Comments", ref _comments, value); }
        }

        [InspectorProperty, JsonProperty]
        public override string Name
        {
            get { return base.Name; }
            set { base.Name = value; }
        }

        private string _typeName;
        private SectionVisibility _visibility;
        private int _row;
        private bool _isNewRow;
        private int _column;
        private int _columnSpan;
        private string _comments;


        //[InspectorProperty, JsonProperty]
        public virtual string TypeName
        {
            get
            {
                return Regex.Replace(Name, @"[^a-zA-Z0-9_\.]+", "");
                if (string.IsNullOrEmpty(_typeName))
                {

                }
                return _typeName;
            }
            set { _typeName = value; }
        }

        public override bool AutoFixName
        {
            get { return false; }
        }

        [InspectorProperty, JsonProperty]
        public SectionVisibility Visibility
        {
            get { return _visibility; }
            set
            {

                this.Changed("Visibility", ref _visibility, value);
            }
        }

        public virtual string ClassName
        {
            get { return this.Node.Name + TypeName; }
        }

        public string ReferenceClassName
        {
            get { return "I" + this.TypeName + "Connectable"; }
        }
        public virtual IEnumerable<IShellNodeConfigItem> IncludedInSections
        {
            get
            {
                return this.OutputsTo<IShellNodeConfigItem>();
            }
        }


    }
}