using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using uFrame.Editor.Attributes;
using uFrame.Editor.Core;
using uFrame.Editor.Graphs.Data;
using uFrame.Json;

namespace uFrame.Architect.Editor.Data
{
    public class ShellGeneratorTypeNode : GenericNode
    {
        private Type _baseType;
        private string _templateType;
        private string _classNameFormat = "{0}";
        private MemberInfo[] _templateMembers;
        private string _folderName;

        public ShellGeneratorTypeNode()
        {
            _templateType = typeof(System.Object).Name;
        }


        public Type BaseType
        {
            get
            {

                return _baseType ?? (BaseType = InvertApplication.FindTypeByName(TemplateType));
            }
            set
            {
                _baseType = value;
                TemplateMembers = null;
            }
        }

        [JsonProperty, InspectorProperty(InspectorType.TypeSelection)]
        public string TemplateType
        {
            get { return _templateType; }
            set
            {
                _templateType = value;
                _baseType = null;
                TemplateMembers = null;
            }
        }

        [JsonProperty, InspectorProperty]
        public bool IsEditorExtension { get; set; }

        [JsonProperty, InspectorProperty]
        public bool IsDesignerOnly { get; set; }

        [JsonProperty, InspectorProperty]
        public string ClassNameFormat
        {
            get { return _classNameFormat; }
            set { _classNameFormat = value; }
        }

        [JsonProperty, InspectorProperty]
        public string FolderName
        {
            get
            {
                if (string.IsNullOrEmpty(_folderName))
                {
                    return Name;
                }
                return _folderName;
            }
            set { _folderName = value; }
        }

        //public ShellNodeGeneratorsSlot ShellNodeShellNodeGeneratorsSlot
        //{
        //    get
        //    {
        //        return this.InputFrom<ShellNodeGeneratorsSlot>();
        //    }
        //}
        public ShellNodeTypeNode GeneratorFor
        {
            get
            {
                var item = this.InputsFrom<MultiOutputSlot<ShellGeneratorTypeNode>>().FirstOrDefault();
                if (item == null) return null;
                return item.Node as ShellNodeTypeNode;
            }
        }
        public IEnumerable<TemplatePropertyReference> Overrides
        {
            get { return PersistedItems.OfType<TemplatePropertyReference>(); }
        }

        public MemberInfo[] TemplateMembers
        {
            get
            {
                if (_templateMembers == null)
                {
                    _templateMembers = BaseType.GetMembers(BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.NonPublic);
                }
                return _templateMembers;
            }
            set { _templateMembers = value; }
        }

        public IEnumerable<TemplatePropertyReference> TemplateProperties
        {
            get { return PersistedItems.OfType<TemplatePropertyReference>(); }
        }
        public IEnumerable<TemplateMethodReference> TemplateMethods
        {
            get { return PersistedItems.OfType<TemplateMethodReference>(); }
        }
        public IEnumerable<TemplateEventReference> TemplateEvents
        {
            get { return PersistedItems.OfType<TemplateEventReference>(); }
        }
        public IEnumerable<TemplateFieldReference> TemplateFields
        {
            get { return PersistedItems.OfType<TemplateFieldReference>(); }
        }

    }
}