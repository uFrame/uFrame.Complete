using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using uFrame.Editor.Compiling.CodeGen;
using uFrame.Editor.Compiling.CommonNodes;
using uFrame.Editor.Configurations;
using uFrame.Editor.Core;
using uFrame.Editor.Graphs.Data;
using uFrame.MVVM.ViewModels;
using UnityEngine;

namespace uFrame.MVVM.Templates
{
    [TemplateClass(TemplateLocation.Both)]
    [AutoNamespaces]
    [NamespacesFromItems]
    public partial class SimpleClassTemplate : IClassTemplate<SimpleClassNode>, ITemplateCustomFilename, IJSonSerializable
    {
        public TemplateContext<SimpleClassNode> Ctx { get; set; }

        public string Filename
        {
            get
            {
                if (Ctx.Data.Name == null)
                {
                    throw new Exception(Ctx.Data.Name + " Graph name is empty");
                }
                return Ctx.IsDesignerFile ? Path2.Combine(Ctx.Data.Graph.Name, "SimpleClasses.designer.cs")
                                          : Path2.Combine(Ctx.Data.Graph.Name + "/SimpleClasses", Ctx.Data.Name + ".cs");
            }
        }

        public string OutputPath { get { return ""; } }

        public bool CanGenerate { get { return true; } }

        private SimpleClassNode SimpleClassNode
        {
            get
            {
                return (SimpleClassNode) Ctx.NodeItem;
            }
        }

        public void TemplateSetup()
        {
            // Support inheritance
            Ctx.CurrentDeclaration.BaseTypes.Clear();
            if (!SimpleClassNode.IsStruct) {
                if (Ctx.IsDesignerFile) {
                    if (SimpleClassNode.BaseNode != null)
                    {
                        Ctx.CurrentDeclaration.BaseTypes.Add((SimpleClassNode.BaseNode.Name).ToCodeReference());
                    }
                    Ctx.CurrentDeclaration.BaseTypes.Add(typeof(IJSonSerializable));
                } else {
                    Ctx.SetBaseType((SimpleClassNode.Node.Name + "Base").ToCodeReference());
                }
            } else {
                if (SimpleClassNode.BaseNode != null)
                    throw new TemplateException(Ctx.Item.Name + " is Struct, but BaseNode = " + SimpleClassNode.BaseNode.FullName);

                Ctx.CurrentDeclaration.IsClass = false;
                Ctx.CurrentDeclaration.IsStruct = true;

                if (Ctx.IsDesignerFile) {
                    Ctx.CurrentDeclaration.Name = Ctx.Data.Node.Name.Clean();
                    Ctx.CurrentDeclaration.BaseTypes.Add(typeof(IJSonSerializable));
                }

                Ctx.CurrentDeclaration.IsPartial = true;
            }

            foreach (var property in Ctx.Data.ChildItemsWithInherited.OfType<ITypedItem>())
            {
                var type = InvertApplication.FindTypeByNameExternal(property.RelatedTypeName);
                if (type == null)
                    continue;

                Ctx.TryAddNamespace(type.Namespace);
            }
            Ctx.AddIterator("Property", node => node.Properties);
            Ctx.AddIterator("Collection", node => node.Collections);
        }

    }

    [RequiresNamespace("uFrame.Kernel")]
    [RequiresNamespace("uFrame.Json")]
    [RequiresNamespace("uFrame.MVVM")]
    [RequiresNamespace("uFrame.MVVM.Bindings")]
    [RequiresNamespace("uFrame.Kernel.Serialization")]
    public partial class SimpleClassTemplate
    {
        [ForEach("Properties"), GenerateProperty, WithField, WithNameFormat("{0}")]
        public _ITEMTYPE_ _Name_Property { get; set; }

        [ForEach("Collections"), GenerateProperty, WithField, WithNameFormat("{0}")]
        public List<_ITEMTYPE_> _Name_Collection { get; set; }

        //Types accepted for the serialization and their serialized identifiers
        public static Dictionary<Type, string> SerializableTypes = new Dictionary<Type, string>
        {
            {typeof (int), "Int"},
            {typeof (Vector3), "Vector3"},
            {typeof (Vector2), "Vector2"},
            {typeof (string), "String"},
            {typeof (bool), "Bool"},
            {typeof (float), "Float"},
            {typeof (double), "Double"},
            {typeof (Quaternion), "Quaternion"},
        };

        [GenerateMethod] //Generate method based on the content
        public string Serialize() //The name and return type will be copied to the result method
        {
            if (!SimpleClassNode.IsStruct)
            {
                Ctx.CurrentMethod.Attributes |= MemberAttributes.Override;
            }

            //Simple output the class. Semicolon (;) is added automatically
            Ctx._("var jsonObject = new JSONClass()");

            //For each is not a pure output. It is actually invoked during generating!!!
            //Here we iterate over properties of the simple class
            foreach (var viewModelPropertyData in Ctx.Data.Properties)
            {
                //Get node of the property if any
                var relatedNode = viewModelPropertyData.RelatedTypeNode;

                //If enum node
                if (relatedNode is EnumNode)
                {
                    //Formatted output. Check the generated code to see how it looks like
                    Ctx._("jsonObject.Add(\"{0}\", new JSONData((int)this.{0}));", viewModelPropertyData.Name);
                }
                else
                {
                    if (viewModelPropertyData.Type == null) continue;
                    if (!SerializableTypes.ContainsKey(viewModelPropertyData.Type)) continue;
                    Ctx._("jsonObject.Add(\"{0}\", new JSONData(this.{0}))",
                        viewModelPropertyData.Name);
                }

            }
            Ctx._("return jsonObject.ToString()");
            return null;
        }

        [GenerateMethod]
        public void Deserialize(string json) {
            if (!SimpleClassNode.IsStruct)
            {
                Ctx.CurrentMethod.Attributes |= MemberAttributes.Override;
            }

            bool addedNodeDeclaration = false;
            Action addNodeDeclaration = () => {
                if (addedNodeDeclaration)
                    return;

                addedNodeDeclaration = true;
                Ctx._("var node = JSON.Parse(json)");
            };

            foreach (var simpleClassPropertyData in Ctx.Data.Properties)
            {

                var relatedNode = simpleClassPropertyData.RelatedTypeNode;
                if (relatedNode is EnumNode) {
                    addNodeDeclaration();
                    Ctx._("this.{0} = ({1})node[\"{0}\"].AsInt", simpleClassPropertyData.Name,
                        simpleClassPropertyData.RelatedTypeName);
                }
                else
                {
                    if (simpleClassPropertyData.Type == null) continue;
                    if (!SerializableTypes.ContainsKey(simpleClassPropertyData.Type)) continue;
                    addNodeDeclaration();
                    Ctx.PushStatements(Ctx._if("node[\"{0}\"] != null", simpleClassPropertyData.Name).TrueStatements);
                    Ctx._("this.{0} = node[\"{0}\"].As{1}", simpleClassPropertyData.Name, SerializableTypes[simpleClassPropertyData.Type]);
                    Ctx.PopStatements();
                }
            }
        }
    }
}


