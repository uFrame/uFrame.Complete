using System.CodeDom;
using uFrame.Editor.Database.Data;
using uFrame.Editor.Graphs.Data;
using uFrame.Editor.Graphs.Data.Types;

namespace uFrame.ECS.Editor
{
    using System;
    using System.Collections.Generic;
   
    public class EnumValueSelection : SelectionFor<IContextVariable, EnumSelectionValue>
    {
        public override bool AllowInputs
        {
            get { return false; }
        }

        public override string ItemDisplayName(IContextVariable item)
        {
            return item.ShortName;
        }

        public TypeSelection ObjectSelector { get; set; }
        public Func<ITypeInfo> VariableTypeSelector { get; set; }

        public override IEnumerable<IValueItem> GetAllowed()
        {
            var item = VariableTypeSelector() as ITypeInfo;
            if (item == null) yield break;
            foreach (var property in item.GetMembers())
            {
                yield return new ContextVariable(item.TypeName, property.MemberName)
                {
                    Node = this.Node,
                    VariableType = new SystemTypeInfo(typeof(int)),
                    Repository = this.Repository,
                };
            }
        }
    }
    public class EnumSelectionValue : InputSelectionValue
    {

    }

    public class EnumValueNode : EnumValueNodeBase {
        private int _value;

        private TypeSelection _typeSelection;

        public TypeSelection Type
        {
            get { return this.GetSlot(ref _typeSelection, "Type",_=>_.Filter =i=>i.IsEnum); }
        }
        
        public override string Title
        {
            get
            {
                if (Repository == null) return string.Empty;

                var item = ValueSelection.Item;
                if (item == null) return "Select A Value";
                return item.VariableName;
            }
        }

        public override string Name
        {
            get { return Title; }
            set { base.Name = value; }
        }

        public override IEnumerable<IGraphItem> GraphItems
        {
            get
            {
                yield return Type;
                yield return ValueSelection;
            }
        }

        private EnumValueSelection _valueSelection;

        public EnumValueSelection ValueSelection
        {
            get
            {
                return GetSlot(ref _valueSelection, "Value", _ => _.VariableTypeSelector = ()=>this.VariableType);
            }
        }

        public override ITypeInfo VariableType
        {
            get
            {
             
               
                return Type.Item as ITypeInfo;
            }
        }

        public IActionIn OutputItem
        {
            get { return this.OutputTo<IActionIn>(); }
        }

        public override string ValueExpression
        {
            get
            {
                return string.Format("{0}", ValueSelection.Item.Name);
            }
        }

        public override CodeExpression GetCreateExpression()
        {
            return new CodeSnippetExpression(ValueExpression);
        }


        public override void Validate(List<ErrorInfo> errors)
        {
            base.Validate(errors);
            if (VariableType == null)
            {
                errors.AddError("Please select an enum type.",this);
            }
            if (ValueSelection.Item == null)
            {
                errors.AddError("Please select a value.", this);
            }
        }
    }



    public partial interface IEnumValueConnectable : IDiagramNodeItem, IConnectable {
    }
}
