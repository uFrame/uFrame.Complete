using uFrame.Attributes;
using uFrame.Editor.Compiling.CodeGen;
using uFrame.Editor.Graphs.Data;
using uFrame.Editor.Graphs.Data.Types;

namespace uFrame.ECS.Editor
{
    using System.Collections.Generic;
    
    [ActionTitle("Create Object"),uFrameCategory("Create")]
    public class CreateObject : CustomAction
    {
        public override string Title
        {
            get
            {
                if (SelectedType != null)
                {
                    return "Create " + SelectedType.TypeName;
                }
                return base.Title;
            }
        }

        private TypeSelection _component;
        private ActionBranch _next;
        private ActionOut _item;
        private VariableIn[] _propertyInputs;
        private VariableOut _result;

        [In]
        public TypeSelection Type
        {
            get
            {
                return GetSlot(ref _component, "Type", _ =>
                {
                    _.Filter = info => info != this;
                });
            }
        }

        public ITypeInfo SelectedType
        {
            get { return Type.Item as ITypeInfo; }
        }

        public VariableIn[] PropertyInputs
        {
            get
            {

                if (SelectedType == null) return null;
                if (_propertyInputs != null)
                {
                    return _propertyInputs;
                }
                List<VariableIn> list = new List<VariableIn>();
                foreach (var item in SelectedType.GetMembers())
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
                if (Repository == null)
                    yield break;

                yield return Type;
                if (Type.Item == null)
                {
                    yield break;
                }
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
            if (SelectedType == null)
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
                        GetInfo = () => SelectedType
                    };
                });
            }
        }
        public override void WriteCode(ISequenceVisitor visitor, TemplateContext ctx)
        {
            base.WriteCode(visitor, ctx);
            var eventVariableName = this.VariableName + "_" + "Object";

            ctx._("var {0} = new {1}()", eventVariableName, SelectedType.TypeName);
            foreach (var item in PropertyInputs)
            {
                ctx._("{0}.{1} = {2}", eventVariableName, item.Name, item.VariableName);
            }
            ctx._("{0} = {1}", Result.VariableName, eventVariableName);
        }
    }


    public class ObjectNode : ObjectNodeBase {
        private TypeSelection _typeSelection;

        public TypeSelection Type
        {
            get { return this.GetSlot(ref _typeSelection, "Type"); }
        }
        public override ITypeInfo VariableType
        {
            get { return Type.Item ?? (SystemTypeInfo)typeof(object); }
        }

        public override IEnumerable<IGraphItem> GraphItems
        {
            get
            {
                yield return Type;
            }
        }
    }

    public partial interface IObjectConnectable : IDiagramNodeItem, IConnectable {
    }
}
