using uFrame.Editor.Attributes;
using uFrame.Json;
using uFrame.Editor.Database.Data;

namespace uFrame.Architect.Editor.Data
{
    public class ShellNodeConfigSection : ShellNodeConfigItem
    {
        private bool _allowAdding;
        private ShellNodeConfigSectionType _sectionType;
        private bool _isTyped;
        private bool _isEditable;
        private bool _allowDuplicates;
        private bool _isAutomatic;
        private bool _hasPredefinedOptions;

        [JsonProperty, InspectorProperty]
        public ShellNodeConfigSectionType SectionType
        {
            get { return _sectionType; }
            set
            {

                this.Changed("SectionType", ref _sectionType, value);
            }
        }

        [InspectorProperty, JsonProperty]
        public bool IsTyped
        {
            get { return _isTyped; }
            set
            {

                this.Changed("IsTyped", ref _isTyped, value);
            }
        }

        [InspectorProperty, JsonProperty]
        public virtual bool AllowAdding
        {
            get
            {
                if (SectionType == ShellNodeConfigSectionType.ChildItems)
                {
                    return true;
                }
                return _allowAdding;
            }
            set
            {

                this.Changed("AllowAdding", ref _allowAdding, value);
            }
        }

        public override string ClassName
        {
            get
            {
                if (SectionType == ShellNodeConfigSectionType.ChildItems)
                {
                    return TypeName + "ChildItem";
                }
                return TypeName + "Reference";
            }
        }


        [InspectorProperty, JsonProperty]
        public bool IsEditable
        {
            get { return _isEditable; }
            set
            {
                this.Changed("IsEditable", ref _isEditable, value);
            }
        }

        [InspectorProperty, JsonProperty]
        public bool AllowDuplicates
        {
            get { return _allowDuplicates; }
            set
            {

                this.Changed("AllowDuplicates", ref _allowDuplicates, value);
            }
        }

        [InspectorProperty, JsonProperty]
        public bool IsAutomatic
        {
            get { return _isAutomatic; }
            set
            {
                this.Changed("IsAutomatic", ref _isAutomatic, value);
            }
        }

        [InspectorProperty, JsonProperty]
        public bool HasPredefinedOptions
        {
            get { return _hasPredefinedOptions; }
            set
            {
                this.Changed("HasPredefinedOptions", ref _hasPredefinedOptions, value);
            }
        }
    }
}