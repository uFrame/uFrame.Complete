using System;
using System.Linq;
using System.Collections.Generic;
using uFrame.Editor.Core;
using uFrame.Editor.Graphs.Data;
using uFrame.IOC;
using uFrame.MVVM.Bindings;
using uFrame.MVVM.Templates;

namespace uFrame.MVVM
{
    public class ViewNode : ViewNodeBase 
    {
        private List<BindingsReference> PossibleBindingReferenceList = new List<BindingsReference>();

        public override IEnumerable<IItem> PossibleBindings
		{
			get
			{
                foreach (var item in Element.BindableProperties)
                {
                    foreach (var mapping in uFrameMVVM.BindingTypes)
                    {
                        var bindableType = mapping.Value as uFrameBindingType;
                        if (bindableType == null) continue;
                        if (!bindableType.CanBind(item)) continue;
                        if (Bindings.FirstOrDefault(p => p.BindingName == mapping.Key.Item2
                                                      && p.BindingType == bindableType
                                                      && p.SourceIdentifier == item.Identifier) != null)
                            continue;

                        BindingsReference bf = PossibleBindingReferenceList.Find(e => e.SourceIdentifier == item.Identifier && e.BindingType == bindableType);
                        if (bf != null)
                        {
                            yield return bf;
                        }
                        else
                        {
                            bf = new BindingsReference()
                            {
                                Repository = item.Repository,
                                SourceIdentifier = item.Identifier,
                                BindingName = mapping.Key.Item2,
                                BindingType = bindableType,
                            };
                            PossibleBindingReferenceList.Add(bf);
                            yield return bf;
                        }
                    }
                }
            }
		}

		public bool IsDerivedOnly
		{
			get
			{
				return this.BaseNode != null && this.ElementInputSlot.InputFrom<ElementNode>() == null;
			}
		}

		public ElementNode Element
		{
			get
			{
                var elementNode = ElementInputSlot.InputFrom<ElementNode>();
                if (elementNode == null)
                {
                    var baseView = BaseNode as ViewNode;
                    if (baseView != null)
                    {
                        return baseView.Element;
                    }

                }
                else
                {
                    return elementNode;
                }

                return null;
            }
		}

		public IEnumerable<PropertiesChildItem> SceneProperties
		{
			get
			{
				return this.ScenePropertiesInputSlot.InputsFrom<PropertiesChildItem>().Distinct();
			}
		}

		public override void Validate(List<ErrorInfo> errors)
		{
			base.Validate(errors);
            if (this.Element == null)
            {
                errors.AddError("This view must have an element.", this);
            }
		}
    }
    
    public partial interface IViewConnectable : IDiagramNodeItem, IConnectable {
    }
}
