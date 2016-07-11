using uFrame.ECS.Editor;
using uFrame.Editor.Configurations;
using uFrame.Editor.Core;
using uFrame.Editor.Database.Data;
using uFrame.Editor.Graphs.Data;
using uFrame.Editor.Graphs.Data.Types;
using uFrame.Editor.TypesSystem;

namespace uFrame.ECS.Editor
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    
    public class PropertyNode : PropertyNodeBase, IContextVariable
    {

        public override void RecordRemoved(IDataRecord record)
        {
            base.RecordRemoved(record);
            var container = this.Filter;
            if (container == null || container.Identifier == record.Identifier)
            {
                Repository.Remove(this);
            }
        }

        private PropertyIn _o;
        private PropertySelection _propertySelection;

        [InputSlot("Object")]
        public PropertyIn Object
        {
            get
            {
                return GetSlot(ref _o,"Object",_=>_.DoesAllowInputs = true);
            }
        } 

        [InputSlot("Property")]
        public PropertySelection PropertySelection
        {
            get
            {
                return GetSlot(ref _propertySelection, "Property", _ => _.ObjectSelector = Object);
            }
        }

        public override IEnumerable<IGraphItem> GraphItems
        {
            get
            {
                yield return Object;
                //if (Object.Item != null)
                //{
                    yield return PropertySelection;
                //}
                
            }
        }

        
        public override string Title
        {
            get
            {
              
                if (Repository == null) return string.Empty;

                var item = PropertySelection.Item;
                if (item == null) return "Select A Property";
                return item.ShortName;
            }
        }

	    public IMemberInfo Source
        {
            get
            {
                if (PropertySelection.Item == null) return null;
                return PropertySelection.Item.Source;
            }
        }

        public string VariableName
        {
            get
            {
                if (PropertySelection.Item == null) return "--Select--";
                return PropertySelection.Item.VariableName;
            }
           
        }

        public ITypeInfo VariableType
        {
            get
            {
                if (PropertySelection.Item == null) return null;
                return PropertySelection.Item.VariableType;
            }
        }

        public string ShortName
        {
            get
            {
                if (PropertySelection.Item == null) return "Property";
                return PropertySelection.Item.ShortName;
            }
        }

        public string ValueExpression
        {
            get
            {
                if (PropertySelection.Item == null) return "null";
                return PropertySelection.Item.VariableName;
            }
        }

        public string Value
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public IEnumerable<IContextVariable> GetPropertyDescriptions()
        {
            if (PropertySelection.Item == null) yield break;
            foreach (var item in  PropertySelection.Item.GetPropertyDescriptions()) yield return item;
        } 

        public override void Validate(List<ErrorInfo> errors)
        {
            base.Validate(errors);
            if (PropertySelection.Item == null)
            {
                errors.AddError("Please select a property.",this);
            }
        }
    }


    public class PropertySelection : SelectionFor<IContextVariable, PropertySelectionValue>
    {
        public override bool AllowInputs
        {
            get { return false; }
        }

        public override string ItemDisplayName(IContextVariable item)
        {
            return item.ShortName;
        }

        public PropertyIn ObjectSelector { get; set; }

        public override IEnumerable<IValueItem> GetAllowed()
        {
            var item = ObjectSelector.Item;
            if (item == null) yield break;
            foreach (var property in item.GetPropertyDescriptions())
            {
                yield return property;
            }
        }
    }

    public class PropertySelectionValue : InputSelectionValue
    {

    }


    public class TypeSelection : SelectionFor<ITypeInfo,TypeSelectionValue>, IActionIn
    {
        public Func<ITypeInfo, bool> Filter { get; set; }
        public override IEnumerable<IValueItem> GetAllowed()
        {

            var list = new List<ITypeInfo>();
            InvertApplication.SignalEvent<IQueryTypes>(_=>_.QueryTypes(list));
            if (Filter == null)
            {
                return list.OfType<IValueItem>();
            }
            return list.Where(Filter).OfType<IValueItem>();
        }

        public IActionFieldInfo ActionFieldInfo { get; set; }
       
        public string VariableName
        {
            get
            {
                
                var actionNode = Node as SequenceItemNode;
                return actionNode.VariableName + "_" + Name;
            }
        }
        public ITypeInfo VariableType { get { return new SystemTypeInfo(typeof(Type)); } }

        public override bool AllowSelection
        {
            get { return true; }
        }

        IContextVariable IActionIn.Item
        {
            get { return new ContextVariable(string.Format("typeof({0})", this.Item.FullName))
            {
                
            }; }
        }
    }

    public interface IQueryTypeSelection
    {
        void QueryTypeSelection(TypeSelection typeSelection, List<TypeSelectionValue> list);
    }
    public class TypeSelectionValue : InputSelectionValue
    {

    }

    public partial interface IPropertyConnectable : IDiagramNodeItem, IConnectable
    {
    }
}
