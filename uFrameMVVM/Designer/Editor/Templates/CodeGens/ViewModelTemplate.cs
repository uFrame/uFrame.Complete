using System;
using System.CodeDom;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using uFrame.Kernel;
using uFrame.Editor.Compiling.CodeGen;
using uFrame.Editor.Compiling.CommonNodes;
using uFrame.Editor.Configurations;
using uFrame.Editor.Core;
using uFrame.Editor.Graphs.Data;
using uFrame.Kernel.Serialization;
using uFrame.MVVM.StateMachines;
using uFrame.MVVM.ViewModels;

namespace uFrame.MVVM.Templates
{
    [TemplateClass(TemplateLocation.Both, ClassNameFormat = uFrameFormats.VIEW_MODEL_FORMAT), AsPartial]
    [AutoNamespaces]
    [NamespacesFromItems]
    public partial class ViewModelTemplate : ViewModel, IClassTemplate<ElementNode>, ITemplateCustomFilename
    {
        public TemplateContext<ElementNode> Ctx { get; set; }

        public string Filename
        {
            get
            {
                if (Ctx.Data.Name == null)
                {
                    throw new Exception(Ctx.Data.Name + " Graph name is empty");
                }
                return Ctx.IsDesignerFile ? Path2.Combine(Ctx.Data.Graph.Name + "/ViewModels.designer", Ctx.Data.Name + "ViewModel.designer.cs")
                                          : Path2.Combine(Ctx.Data.Graph.Name + "/ViewModels", Ctx.Data.Name + "ViewModel.cs");
            }
        }

        // Replace by ITemplateCustomFilename's Filename
        public string OutputPath { get { return ""; } }

        public bool CanGenerate { get { return true; } }

        public ITypedItem[] ViewModelProperties { get; set; }

        public ITypedItem[] StateMachineProperties { get; set; }

        public void TemplateSetup()
        {
            foreach (var property in Ctx.Data.PersistedItems.OfType<ITypedItem>())
            {
                var type = InvertApplication.FindTypeByNameExternal(property.RelatedTypeName);
                if (type == null)
                    continue;
                Ctx.TryAddNamespace(type.Namespace);
            }

            StateMachineProperties = Ctx.Data.AllProperties.Where(p => (p.RelatedNode() is StateMachineNode)).ToArray();
            ViewModelProperties = Ctx.Data.AllProperties.Where(p => !StateMachineProperties.Contains(p)).ToArray();
            if (!Ctx.IsDesignerFile) return;
            foreach(var item in Ctx.Data.ComputedProperties)
            {
                Ctx.CurrentDeclaration._private_(typeof(IDisposable), "_{0}Disposable", item.Name);
            }
        }

    }

    [RequiresNamespace("uFrame.IOC")]
    [RequiresNamespace("uFrame.Kernel")]
    [RequiresNamespace("uFrame.MVVM")]
    [RequiresNamespace("uFrame.MVVM.ViewModels")]
    [RequiresNamespace("uFrame.MVVM.Bindings")]
    [RequiresNamespace("uFrame.Kernel.Serialization")]
    [RequiresNamespace("UnityEngine")]
    [RequiresNamespace("UniRx")]
    public partial class ViewModelTemplate
    {
        [GenerateConstructor(TemplateLocation.DesignerFile, "aggregator")]
        public void AggregatorConstructor(IEventAggregator aggregator)
        {
        }

        [GenerateMethod]
        public override void Bind()
        {
            if (!Ctx.IsDesignerFile)
                return;

            foreach (var command in Ctx.Data.LocalCommands)
            {
                Ctx._("this.{0} = new Signal<{0}Command>(this)", command.Name);
            }


            foreach (var property in ViewModelProperties)
            {
                Ctx._("{0} = new P<{1}>(this, \"{2}\")", property.Name.AsSubscribableField(), property.RelatedTypeName, property.Name);
            }

            // No more parents so no need to bind to the collection change, this was bad anyways
            foreach (var property in Ctx.Data.LocalCollections)
            {
                Ctx._("{0} = new ModelCollection<{1}>(this, \"{2}\")", property.Name.AsField(), property.RelatedTypeName,
                    property.Name);
                //   Ctx._("{0}.CollectionChanged += {1}CollectionChanged", property.Name.AsField(), property.Name);
            }

            foreach (var item in StateMachineProperties)
            {
                Ctx._("{0} = new {1}(this, \"{2}\")", item.Name.AsSubscribableField(), item.RelatedTypeName, item.Name);
            }

            foreach (var item in Ctx.Data.ComputedProperties)
            {
                Ctx._("Reset{0}()", item.Name);
            }

            // TODO : State Machine Relate
            foreach (var item in Ctx.Data.ComputedProperties)
            {
                var transition = item.OutputTo<TransitionsChildItem>();
                if (transition == null) continue;
                var stateMachineNode = transition.Node as IClassTypeNode;
                var property = stateMachineNode.ReferencesOf<PropertiesChildItem>().FirstOrDefault(p => p.Node == Ctx.Data);
                if (property == null) continue;
                Ctx._("{0}.{1}.AddComputer({2})", property.Name.AsSubscribableProperty(), transition.Name,
                    item.Name.AsSubscribableProperty());
            }

            foreach (var item in Ctx.Data.LocalCommands)
            {
                ////Transition.Subscribe(_ => StateMachineProperty.B.OnNext(true));
                var transition = item.OutputTo<TransitionsChildItem>();
                if (transition == null) continue;
                var stateMachineNode = transition.Node as IClassTypeNode;
                var property = stateMachineNode.ReferencesOf<PropertiesChildItem>().FirstOrDefault(p => p.Node == Ctx.Data);
                if (property == null) continue;
                Ctx._("{0}.Subscribe(_ => {1}.{2}.OnNext(true))", item.Name, property.Name.AsSubscribableProperty(),
                    transition.Name);
            }
        }

        #region StateMachineProperties
        //[TemplateProperty(uFrameFormats.SUBSCRIBABLE_PROPERTY_FORMAT, AutoFillType.NameAndTypeWithBackingField)]
        [ForEach("StateMachineProperties"), GenerateProperty, WithField]
        public virtual _ITEMTYPE_ _Name2_Property
        {
            get { return null; }
        }

        //[TemplateProperty(TemplateLocation.DesignerFile, AutoFillType.NameOnly)]
        [ForEach("StateMachineProperties"), GenerateProperty]
        public virtual State _Name2_
        {
            get
            {
                Ctx._("return {0}.Value", Ctx.Item.Name.AsSubscribableProperty());
                return null;
            }
            set { Ctx._("{0}.Value = value", Ctx.Item.Name.AsSubscribableProperty()); }
        }
        #endregion

        #region ViewModelProperties

        [ForEach("ViewModelProperties"), GenerateProperty, WithField]
        public virtual P<_ITEMTYPE_> _Name_Property
        {
            get { return null; }

        }

        [ForEach("ViewModelProperties"), GenerateProperty]
        public virtual _ITEMTYPE_ _Name_
        {
            get
            {
                Ctx._("return {0}.Value", Ctx.Item.Name.AsSubscribableProperty());
                return null;
            }
            set { Ctx._("{0}.Value = value", Ctx.Item.Name.AsSubscribableProperty()); }
        }

        #endregion

        #region Collections

        [ForEach("LocalCollections"), GenerateProperty, WithField]
        public virtual ModelCollection<_ITEMTYPE_> _Name4_
        {
            get { return null; }
        }

        #endregion

        #region Commands

        [ForEach("LocalCommands"), GenerateProperty, WithField]
        public virtual object _Name3_
        {
            get
            {
                Ctx.SetType("Signal<{0}Command>", Ctx.Item.Name);
                return null;
            }
            set { }
        }

        public IEnumerable<CommandsChildItem> ChildCommands
        {
            get { return Ctx.Data.LocalCommands.Where(c => c.OutputCommand == null); }
        }

        public IEnumerable<CommandsChildItem> NodeCommands
        {
            get { return Ctx.Data.LocalCommands.Where(c => c.OutputCommand != null); }
        }


        [ForEach("ChildCommands"), GenerateMethod]
        public virtual void Execute_Name_()
        {
            var cmd = Ctx.ItemAs<CommandsChildItem>();

            if(cmd.HasArgument)
            {
                Ctx.CurrentMethod.Parameters.Add(new CodeParameterDeclarationExpression(cmd.RelatedTypeName, "argument"));
            }

            if (!cmd.HasArgument)
            {
                Ctx._("this.{0}.OnNext(new {1}())", cmd.Name, cmd.ClassName);
            }
            else
            {
                Ctx._("this.{0}.OnNext(new {1}(){{" +
                    "Argument = argument" +
                    "}})", cmd.Name, cmd.ClassName);
            }
        }

        [ForEach("NodeCommands"), GenerateMethod]
        public virtual void Execute()
        {
            var cmd = Ctx.ItemAs<CommandsChildItem>();
            Ctx.CurrentMethod.Parameters.Add(new CodeParameterDeclarationExpression(cmd.RelatedTypeName, "argument"));
            Ctx._("this.{0}.OnNext(argument)", cmd.Name);
        }

        #endregion

        #region Serialization

        [GenerateMethod]
        public override void Read(ISerializerStream stream)
        {
            foreach (var viewModelPropertyData in Ctx.Data.LocalProperties)
            {
                var relatedNode = viewModelPropertyData.RelatedTypeNode;
                if (relatedNode is EnumNode)
                {
                    Ctx._("this.{0} = ({1})stream.DeserializeInt(\"{0}\");", viewModelPropertyData.Name,
                        viewModelPropertyData.RelatedTypeName);
                }
                else if (relatedNode is ElementNode)
                {
                    var elementNode = relatedNode as ElementNode;
                    Ctx._("\t\tif (stream.DeepSerialize) this.{0} = stream.DeserializeObject<{1}>(\"{0}\");",
                        viewModelPropertyData.Name, elementNode.Name.AsViewModel());

                }
                else if (relatedNode is StateMachineNode)
                {
                    Ctx._("this.{0}.SetState(stream.DeserializeString(\"{1}\"))", viewModelPropertyData.FieldName,
                        viewModelPropertyData.Name);
                }
                else
                {
                    if (viewModelPropertyData.Type == null) continue;
                    if (!AcceptableTypes.ContainsKey(viewModelPropertyData.Type)) continue;
                    Ctx._("this.{0} = stream.Deserialize{1}(\"{0}\");", viewModelPropertyData.Name,
                        AcceptableTypes[viewModelPropertyData.Type]);
                }
            }
            foreach (var collection in Ctx.Data.LocalCollections)
            {
                var relatedNode = collection.RelatedTypeNode;
                if (relatedNode is ElementNode)
                {
                    var elementNode = relatedNode as ElementNode;

                    Ctx.PushStatements(Ctx._if("stream.DeepSerialize").TrueStatements);
                    Ctx._("this.{0}.Clear()", collection.Name);
                    Ctx._("this.{0}.AddRange(stream.DeserializeObjectArray<{1}>(\"{0}\"))", collection.Name, elementNode.Name.AsViewModel());
                    Ctx.PopStatements();
                }
            }
        }

        [GenerateMethod]
        public override void Write(ISerializerStream stream)
        {
            foreach (var viewModelPropertyData in Ctx.Data.LocalProperties)
            {

                var relatedNode = viewModelPropertyData.RelatedTypeNode;
                if (relatedNode is EnumNode)
                {
                    Ctx._("stream.SerializeInt(\"{0}\", (int)this.{0});", viewModelPropertyData.Name);

                }
                else if (relatedNode is ElementNode)
                {
                    Ctx._("if (stream.DeepSerialize) stream.SerializeObject(\"{0}\", this.{0});",
                        viewModelPropertyData.Name);
                }
                else if (relatedNode is StateMachineNode)
                {
                    Ctx._("stream.SerializeString(\"{0}\", this.{0}.Name);", viewModelPropertyData.Name);
                }
                else
                {
                    if (viewModelPropertyData.Type == null) continue;
                    if (!AcceptableTypes.ContainsKey(viewModelPropertyData.Type)) continue;
                    Ctx._("stream.Serialize{0}(\"{1}\", this.{1})", AcceptableTypes[viewModelPropertyData.Type],
                        viewModelPropertyData.Name);
                }
            }
            foreach (var collection in Ctx.Data.LocalCollections)
            {
                var relatedNode = collection.RelatedTypeNode;
                if (relatedNode is ElementNode)
                {
                    Ctx._("if (stream.DeepSerialize) stream.SerializeArray(\"{0}\", this.{0})",
                        collection.Name);
                }
            }
        }

        #endregion

        #region Reflection

        [GenerateMethod]
        protected override void FillCommands(List<ViewModelCommandInfo> list)
        {
            //base.FillCommands(list);
            foreach (var commandChildItem in Ctx.Data.LocalCommands)
            {
                Ctx._("list.Add(new ViewModelCommandInfo(\"{0}\", {0}) {{ ParameterType = typeof({1}) }})",
                    commandChildItem.Name,
                    !commandChildItem.HasArgument ? "void" : commandChildItem.RelatedTypeName
                    );
            }

        }


        [GenerateMethod]
        protected override void FillProperties(List<ViewModelPropertyInfo> list)
        {
            //base.FillProperties(list);
            foreach (var property in Ctx.Data.AllProperties)
            {
                Ctx._comment(property.GetType().Name);
                Ctx._("list.Add(new ViewModelPropertyInfo({0}, {1}, {2}, {3}, {4}))",
                    property.Name.AsSubscribableField(),
                    property.RelatedNode() is ElementNode ? "true" : "false",
                    "false",
                    property.RelatedNode() is EnumNode ? "true" : "false", // TODO FOR ENUMS
                    property is ComputedPropertyNode ? "true" : "false"
                    );
            }
            foreach (var property in Ctx.Data.LocalCollections)
            {
                Ctx._("list.Add(new ViewModelPropertyInfo({0}, {1}, {2}, {3}, {4}))",
                    property.Name.AsField(),
                    property.RelatedNode() is ElementNode ? "true" : "false",
                    "true",
                    property.RelatedNode() is EnumNode ? "true" : "false", // TODO FOR ENUMS
                    "false"
                    );
            }
        }

        #endregion

        // [TemplateMethod(TemplateLocation.DesignerFile, AutoFill = AutoFillType.NameOnly, NameFormat = "Get{0}Dependents")]
        [ForEach("ComputedProperties"), GenerateMethod]
        public virtual IEnumerable<IObservableProperty> Get_Name_Dependents()
        {
            var computed = Ctx.ItemAs<ComputedPropertyNode>();
            foreach (var item in computed.InputsFrom<PropertiesChildItem>())
            {
                if (!computed.SubProperties.Any())
                {
                    Ctx._("yield return {0}", item.Name.AsSubscribableProperty());
                    continue;
                }

                var relatedNode = item.RelatedTypeNode;
                if (relatedNode == null) continue;

                Ctx._("{0}.Take(1).Subscribe(_ => Reset{1}())", item.Name.AsSubscribableProperty(), computed.Name);
                var conditionStatements = Ctx._if("{0}.Value != null", item.Name.AsSubscribableProperty()).TrueStatements;
                foreach (var p in computed.SubProperties.Where(p => p.SourceItem.Node == relatedNode))
                {
                    conditionStatements._("yield return {0}.Value.{1}", item.Name.AsSubscribableProperty(),
                        p.Name.AsSubscribableProperty());
                }
            }
            foreach (var item in computed.InputsFrom<ComputedPropertyNode>())
            {
                Ctx._("yield return {0}", item.Name.AsSubscribableField());
            }


            Ctx._("yield break");

            yield break;
        }

        //[TemplateMethod(TemplateLocation.DesignerFile, AutoFill = AutoFillType.NameOnly, NameFormat = "Reset{0}")]
        [ForEach("ComputedProperties"), GenerateMethod]
        public virtual void Reset_Name_()
        {
            var computed = Ctx.ItemAs<ComputedPropertyNode>();
            Ctx._if("_{0}Disposable != null", computed.Name)
                .TrueStatements._("_{0}Disposable.Dispose()", computed.Name);
            Ctx._("_{0}Disposable = {1}.ToComputed(Compute{0}, this.Get{0}Dependents().ToArray()).DisposeWith(this)",
                computed.Name, computed.Name.AsSubscribableField());

        }

        //[TemplateMethod(TemplateLocation.Both, AutoFill = AutoFillType.NameOnly, NameFormat = "Compute{0}")]
        [ForEach("ComputedProperties"), GenerateMethod]
        public virtual bool Compute_Name_()
        {
            var type = Ctx.ItemAs<ComputedPropertyNode>().RelatedTypeName;
            Ctx.SetType(type);
            Ctx._("return default({0})", type);
            return false;
        }

        public static Dictionary<Type, string> AcceptableTypes = new Dictionary<Type, string>
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
    }
}

