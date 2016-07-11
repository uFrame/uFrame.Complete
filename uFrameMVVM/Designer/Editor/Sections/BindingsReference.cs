using System;
using System.Linq;
using System.Collections.Generic;
using uFrame.Editor.Graphs.Data;
using uFrame.IOC;
using uFrame.Json;
using uFrame.MVVM.Templates;
using uFrame.Editor.Core;

namespace uFrame.MVVM
{
    public class BindingsReference : BindingsReferenceBase 
    {
        private uFrameBindingType _bindingType;

        public override bool AllowInputs
        {
            get
            {
                return false;
            }
        }

        [JsonProperty]
        public string BindingName
        {
            get;
            set;
        }

        public override string Name
        {
            get
            {
                return this.Title;
            }
            set
            {
                base.Name = value;
            }
        }

        public uFrameBindingType BindingType
        {
            get
			{
                return _bindingType ?? (_bindingType = uFrameMVVM.BindingTypes
                                                                 .Where(p => p.Key.Item2 == BindingName)
                                                                 .Select(p => p.Value).FirstOrDefault() as uFrameBindingType);
            }
            set
            {
                this._bindingType = value;
            }
        }

        public override string Title
        {
            get
            {
                return SourceItem == null ? "Error: Bindable Not Found" : string.Format(BindingType.DisplayFormat, SourceItem.Name);
            }
        }

        public override string Group
        {
            get
            {
                return string.Format(this.BindingType.DisplayFormat, "{Item}");
            }
        }
    }
    
    public partial interface IBindingsConnectable : IDiagramNodeItem, IConnectable, IItem, ISelectable {
    }
}
