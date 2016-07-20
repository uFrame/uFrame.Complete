using System.CodeDom;
using System.Text.RegularExpressions;
using uFrame.Attributes;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using uFrame.Editor.Attributes;
using uFrame.Editor.Compiling.CodeGen;
using uFrame.Editor.Database.Data;
using uFrame.Editor.Graphs.Data;
using uFrame.Editor.Graphs.Data.Types;
using uFrame.Json;

namespace uFrame.ECS.Editor
{
    public class DynamicTypeInfo : ITypeInfo
    {
        public static SystemTypeInfo ObjectInfo = new SystemTypeInfo(typeof(object));

        public Func<ITypeInfo> GetInfo;

        protected ITypeInfo Info
        {
            get { return GetInfo() ?? ObjectInfo; }
        }

        public IEnumerable<IMemberInfo> GetMembers()
        {
            return Info.GetMembers();
        }

        public bool IsAssignableTo(ITypeInfo info)
        {
            return Info.IsAssignableTo(info);
        }

        public ITypeInfo BaseTypeInfo
        {
            get { return Info.BaseTypeInfo; }
        }

        public bool HasAttribute(Type attribute)
        {
            return Info.HasAttribute(attribute);
        }

        public bool IsArray { get { return Info.IsArray; } }

        public bool IsList
        {
            get { return Info.IsList; }
        }

        public bool IsEnum
        {
            get { return Info.IsEnum; }
        }

        public ITypeInfo InnerType
        {
            get { return Info.InnerType; }
        }

        public string TypeName
        {
            get { return Info.TypeName; }
        }

        public string FullName { get { return Info.FullName; } }
        public string Namespace { get { return Info.Namespace; } }
        public string Title { get { return Info.TypeName; } }
        public string Group { get { return Info.Namespace; } }
        public string SearchTag { get { return FullName; } }
        public string Description { get; set; }
        public string Identifier { get { return FullName; } set { } }
    }
    public class LoopCollectionNode : LoopCollectionNodeBase, IConnectableProvider
    {

        private VariableIn _list;
        private ActionBranch _next;
        private ActionOut _item;
        public override IEnumerable<IContextVariable> GetContextVariables()
        {
            yield return Item;
        }

        public override string Name
        {
            get { return "Loop Collection"; }
            set { base.Name = value; }
        }

        public VariableIn List
        {
            get
            {
                return GetSlot(ref _list, "List", _ =>
                    {
                        _.DoesAllowInputs = true;

                    });
            }
        }

        public ActionBranch Next
        {
            get { return GetSlot(ref _next, "Next"); }
        }

        public ActionOut Item
        {
            get
            {
                return GetSlot(ref _item, "Item", _ =>
                {
                    _.VariableType = new DynamicTypeInfo()
                    {
                        GetInfo = () =>
                        {
                            return List.Item == null || List.Item.VariableType == null ? null : List.Item.VariableType.InnerType;
                        }
                    };
                });
            }
        }

        public override void WriteCode(ISequenceVisitor visitor, TemplateContext ctx)
        {
            base.WriteCode(visitor, ctx);

            var loop = new CodeIterationStatement(
                new CodeSnippetStatement(string.Format("var {0}Index = 0", Item.VariableName)),
                new CodeSnippetExpression(string.Format("{0}Index < {1}.Count", Item.VariableName, List.VariableName)),
                new CodeSnippetStatement(string.Format("{0}Index++", Item.VariableName))
                );

            loop.Statements._("{0} = {1}[{0}Index]", Item.VariableName, List.VariableName);
            ctx.PushStatements(loop.Statements);
            Next.WriteInvoke(ctx);
            ctx.PopStatements();
            ctx.CurrentStatements.Add(loop);
        }

        public override IEnumerable<IGraphItem> GraphItems
        {
            get
            {
                yield return List;
                if (List.Item != null)
                {
                    yield return Next;
                    yield return Item;
                }
            }
        }

        public IEnumerable<IConnectable> Connectables
        {
            get
            {
                if (Repository == null)
                {
                    yield break;
                }
                yield return List;
                yield return Item;

            }
        }
    }

    [ActionTitle("Loop Group Items"), uFrameCategory("Loops")]
    public class LoopGroupNode : CustomAction
    {

        private TypeSelection _list;
        private ActionBranch _next;
        private ActionOut _item;

        [In]
        public TypeSelection List
        {
            get
            {
                return GetSlot(ref _list, "Group", _ =>
                {
                    _.Filter = info => info is ComponentNode || info is GroupNode;

                });
            }
        }

        [Out]
        public ActionBranch Next
        {
            get { return GetSlot(ref _next, "Next"); }
        }

        [In]
        public ActionOut Item
        {
            get
            {
                return GetSlot(ref _item, "Item", _ =>
                {
                    _.VariableType = new DynamicTypeInfo()
                    {
                        GetInfo = () =>
                        {
                            return List.Item;
                        }
                    };
                });
            }
        }

        public override void WriteCode(ISequenceVisitor visitor, TemplateContext ctx)
        {
            base.WriteCode(visitor, ctx);
            ctx._("var {0}Components = System.ComponentSystem.RegisterComponent<{1}>().Components", List.VariableName, List.Item.FullName);

            var loop = new CodeIterationStatement(
                new CodeSnippetStatement(string.Format("var {0}Index = 0", Item.VariableName)),
                new CodeSnippetExpression(string.Format("{0}Index < {1}Components.Count", Item.VariableName, List.VariableName)),
                new CodeSnippetStatement(string.Format("{0}Index++", Item.VariableName))
                );

            loop.Statements._("{0} = {1}Components[{0}Index]", Item.VariableName, List.VariableName);
            ctx.PushStatements(loop.Statements);
            Next.WriteInvoke(ctx);
            ctx.PopStatements();
            ctx.CurrentStatements.Add(loop);
        }


    }
    [ActionTitle("Add Component"), uFrameCategory("Components")]
    public class AddComponentNode : CustomAction
    {

        private TypeSelection _component;
        private ActionBranch _next;
        private ActionOut _item;
        private VariableIn[] _propertyInputs;
        private VariableOut _result;
        private VariableIn _gameObject;

        public override void Validate(List<ErrorInfo> errors)
        {
            base.Validate(errors);
            if (SelectedComponent == null)
            {
                errors.AddError("Component must be set.", this);
            }
            if (GameObject.Item == null)
            {
                errors.AddError("GameObject must be set.", this);
            }
        }
        [In]
        public TypeSelection Component
        {
            get
            {
                return GetSlot(ref _component, "Component", _ =>
                {
                    _.Filter = info => info is ComponentNode;
                });
            }
        }

        [In]
        public VariableIn GameObject
        {
            get
            {
                return GetSlot(ref _gameObject, "GameObject", _ =>
                    {
                        _.DoesAllowInputs = true;
                        _.VariableType = new SystemTypeInfo(typeof(GameObject));
                    });
            }
        }

        [Out]
        public VariableOut Result
        {
            get
            {
                return GetSlot(ref _result, "Result", _ =>
                {
                    _.VariableType = new DynamicTypeInfo()
                    {
                        GetInfo = () => SelectedComponent
                    };
                });
            }
        }

        public ComponentNode SelectedComponent
        {
            get { return Component.Item as ComponentNode; }
        }

        public VariableIn[] PropertyInputs
        {
            get
            {
                if (SelectedComponent == null) return null;
                List<VariableIn> list = new List<VariableIn>();
                foreach (IDiagramNodeItem item in SelectedComponent.PersistedItems)
                {
                    GenericTypedChildItem p = item as GenericTypedChildItem;
                    if (p != null)
                    {
                        var variableIn = CreateSlot<VariableIn>(p.MemberName, p.Identifier);
                        variableIn.DoesAllowInputs = true;
                        list.Add(variableIn);
                    }
                }
                return _propertyInputs ?? (_propertyInputs = list
                    .ToArray()
                    );
            }

        }

        public override IEnumerable<IGraphItem> GraphItems
        {
            get
            {
                yield return GameObject;
                yield return Component;

                if (PropertyInputs != null)
                    foreach (var item in PropertyInputs)
                    {
                        yield return item;
                    }

                yield return Result;
            }
        }

        public override void WriteCode(ISequenceVisitor visitor, TemplateContext ctx)
        {
            base.WriteCode(visitor, ctx);
            var eventVariableName = this.VariableName + "_" + "Component";

            ctx._("var {0} = {1}.AddComponent<{2}>()", eventVariableName, GameObject.VariableName, SelectedComponent.TypeName);
            
            foreach (var item in PropertyInputs)
            {
                ctx._("{0}.{1} = {2}", eventVariableName, item.Name, item.VariableName);
            }

            ctx._("{0} = {1}", eventVariableName, Result.VariableName);
        }



    }

    

    [ActionTitle("Publish Event"), uFrameCategory("Events"),ActionDescription("Publish event of any type and provide any data specified by the event.")]
    public class PublishEventNode : CustomAction
    {
        public override string Title
        {
            get
            {
                if (Event.Item == null) return "Publish ...";
                return "Publish " + Event.Item.TypeName;
            }
        }

        private TypeSelection _component;
        private ActionBranch _next;
        private ActionOut _item;
        private VariableIn[] _propertyInputs;
        private VariableOut _result;

        [In]
        public TypeSelection Event
        {
            get
            {
                return GetSlot(ref _component, "Event", _ =>
                {
                    _.Filter = info => info is EventNode || info.HasAttribute(typeof(uFrameEvent));
                });
            }
        }

        public ITypeInfo SelectedEvent
        {
            get { return Event.Item as ITypeInfo; }
        }

        public VariableIn[] PropertyInputs
        {
            get
            {

                if (SelectedEvent == null) return null;
                if (_propertyInputs != null)
                {
                    return _propertyInputs;
                }
                List<VariableIn> list = new List<VariableIn>();
                foreach (var item in SelectedEvent.GetMembers())
                {
                    if (item is IMethodMemberInfo) continue;
                    var variableIn = CreateSlot<VariableIn>(item.MemberName, item.MemberName);
                    variableIn.DoesAllowInputs = true;
                    variableIn.VariableType = item.MemberType;
                    list.Add(variableIn);
                }
                return _propertyInputs = list.ToArray();
            }

        }

        public override IEnumerable<IGraphItem> GraphItems
        {
            get
            {
                yield return Event;
                if (PropertyInputs != null)
                    foreach (var item in PropertyInputs)
                    {
                        yield return item;
                    }
                yield return Result;
            }
        }

        public override void Validate(List<ErrorInfo> errors)
        {
            base.Validate(errors);
            if (SelectedEvent == null)
            {
                errors.AddError("Event must be set.", this);
            }
        }
        [Out]
        public VariableOut Result
        {
            get
            {
                return GetSlot(ref _result, "Result", _ =>
                {
                    _.VariableType = new DynamicTypeInfo()
                    {
                        GetInfo = () => SelectedEvent
                    };
                });
            }
        }
        public override void WriteCode(ISequenceVisitor visitor, TemplateContext ctx)
        {
            base.WriteCode(visitor, ctx);
            var eventVariableName = this.VariableName + "_" + "Event";

            ctx._("var {0} = new {1}()", eventVariableName, SelectedEvent.TypeName);
            foreach (var item in PropertyInputs)
            {
                ctx._("{0}.{1} = {2}", eventVariableName, item.Name, item.VariableName);
            }
            ctx._("System.Publish({0})", eventVariableName);
            ctx._("{0} = {1}", Result.VariableName, eventVariableName);
        }


    }



    [ActionTitle("Format String")]
    public class FormatStringAction : CustomAction
    {
        [JsonProperty, NodeProperty(InspectorType.TextArea)]
        public string Format
        {
            get { return _s; }
            set
            {
                _formatInputs = null;
                this.Changed("Format", ref _s, value);

            }
        }
        [Out]
        public VariableOut Item
        {
            get
            {
                return GetSlot(ref _item, "Result", _ => _.VariableType = new SystemTypeInfo(typeof(string)));
            }
        }
        public IEnumerable<string> Options
        {
            get
            {
                if (string.IsNullOrEmpty(Format)) yield break;
                var matches = Regex.Matches(Format, @"\{.+?\}").Count;
                for (var i = 0; i < matches; i++)
                    yield return "Item " + i;
            }
        }

        private string _s = "{0}";
        private VariableIn[] _formatInputs;
        private VariableOut _item;

        public VariableIn[] FormatInputs
        {
            get
            {
                if (_formatInputs != null) return _formatInputs;
                else
                {
                    List<VariableIn> list = new List<VariableIn>();

                    var x = 0;
                    foreach (var p in Options)
                    {
                        var slot = CreateSlot<VariableIn>(p);
                        // TODO : Description Change Cause StackOverFlow issue
                        //slot.Description = string.Format("This item will be cast to a string and used as {0} argument of the format string", x++);
                        slot.DoesAllowInputs = true;
                        list.Add(slot);
                    }


                    return _formatInputs = list.ToArray();
                }
            }
            set { _formatInputs = value; }
        }

        public override IEnumerable<IGraphItem> GraphItems
        {
            get
            {
                foreach (var item in FormatInputs)
                {
                    yield return item;
                }
                yield return Item;
            }
        }

        public override void Validate(List<ErrorInfo> errors)
        {
            base.Validate(errors);
            foreach (var item in FormatInputs)
            {
                if (item.Item == null)
                {
                    errors.AddError(item.Name + " is required", item);
                }
            }
        }

        public override void WriteCode(ISequenceVisitor visitor, TemplateContext ctx)
        {
            base.WriteCode(visitor, ctx);
            if (!string.IsNullOrEmpty(Format))
            {
                ctx._("{0} = string.Format(@\"{1}\", {2})", Item.VariableName, JSONNode.Escape(Format), string.Join(",", FormatInputs.Select(p => p.VariableName).ToArray()));
            }

        }
    }

    public class ListAction : CustomAction
    {

        private VariableIn _list;
        [In]
        public virtual VariableIn List
        {
            get
            {
                return GetSlot(ref _list, "List", _ =>
                {
                    _.DoesAllowInputs = true;

                });
            }
        }


        public override void Validate(List<ErrorInfo> errors)
        {
            base.Validate(errors);
            if (List.Item == null)
            {
                errors.AddError("List is required.", this);
            }
        }

    }

    public class ListActionWithItem : ListAction
    {
        [In]
        public override VariableIn List
        {
            get { return base.List; }
        }
        private VariableIn _item;

        [In]
        public VariableIn Item
        {
            get
            {
                return GetSlot(ref _item, "Item", _ =>
                {
                    _.DoesAllowInputs = true;
                });
            }
        }

        public override void Validate(List<ErrorInfo> errors)
        {
            base.Validate(errors);
            if (Item.Item == null)
            {
                errors.AddError("Item is required.", this);
            }
        }
    }
    [ActionTitle("Add To List"), uFrameCategory("Lists", "Collections")]
    public class ListAdd : ListActionWithItem
    {

        public override void WriteCode(ISequenceVisitor visitor, TemplateContext ctx)
        {
            base.WriteCode(visitor, ctx);
            ctx._("{0}.Add({1})", List.VariableName, Item.VariableName);
        }

    }
    [ActionTitle("Remove From List"), uFrameCategory("Lists", "Collections")]
    public class ListRemove : ListActionWithItem
    {
        public override void WriteCode(ISequenceVisitor visitor, TemplateContext ctx)
        {
            base.WriteCode(visitor, ctx);
            ctx._("{0}.Remove({1})", List.VariableName, Item.VariableName);
        }
    }

    [ActionTitle("Get List Item"), uFrameCategory("Lists", "Collections")]
    public class GetListItem : ListAction
    {
        private VariableIn _indexVariable;
        private VariableOut _result;

        [In]
        public VariableIn IndexVariable
        {
            get { return GetSlot(ref _indexVariable, "Index"); }
        }

        [Out]
        public VariableOut Result
        {
            get
            {
                return GetSlot(ref _result, "Result", _ =>
                {
                    _.VariableType = new DynamicTypeInfo()
                    {
                        GetInfo = () => List.Item == null || List.Item.VariableType == null ? null : List.Item.VariableType.InnerType
                    };
                });
            }
        }

        public override void WriteCode(ISequenceVisitor visitor, TemplateContext ctx)
        {
            base.WriteCode(visitor, ctx);
            ctx._("{0} = {1}[{2}]", Result.VariableName, List.VariableName, IndexVariable.VariableName);
        }
    }

    public partial interface ILoopCollectionConnectable : IDiagramNodeItem, IConnectable
    {
    }

    [ActionTitle("Get Random List Item"), uFrameCategory("Lists", "Collections")]
    public class GetRandomListItem : ListAction
    {
        private VariableOut _result;

        [Out]
        public VariableOut Result
        {
            get
            {
                return GetSlot(ref _result, "Result", _ =>
                {
                    _.VariableType = new DynamicTypeInfo()
                    {
                        GetInfo = () => List.Item == null || List.Item.VariableType == null ? null : List.Item.VariableType.InnerType
                    };
                });
            }
        }

        public override void WriteCode(ISequenceVisitor visitor, TemplateContext ctx)
        {
            base.WriteCode(visitor, ctx);

            ctx._("{0} = {1}[UnityEngine.Random.Range(0, {1}.Count)]", Result.VariableName, List.VariableName);
        }
    }
}
