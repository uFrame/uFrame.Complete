using uFrame.Editor.Graphs.Data;
using uFrame.MVVM.Templates;

namespace uFrame.MVVM
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using uFrame.Editor.Configurations;


    public class ElementNode : ElementNodeBase
    {
        public override void Validate(List<ErrorInfo> errors)
        {
            base.Validate(errors);
            var ps = ChildItemsWithInherited.OfType<ITypedItem>().ToArray();
            foreach (var p1 in ps)
            {
                foreach (var p2 in ps)
                {
                    if (p1.Name == p2.Name && p1 != p2)
                    {
                        errors.AddError(string.Format("Duplicate \"{0}\"", p1.Name), this);
                    }
                }
            }
        }

        public IEnumerable<ElementNode> RelatedElements
        {
            get
            {
                return this.Graph.AllGraphItems
                                 .OfType<SubSystemNode>()
                                 .SelectMany(p => p.Children)
                                 .OfType<ElementNode>()
                                 .Distinct();
            }
        }

        public virtual IEnumerable<ComputedPropertyNode> ComputedProperties
        {
            get
            {
                return this.Children.OfType<ComputedPropertyNode>();
            }
        }

        public IEnumerable<InstancesReference> RegisteredInstances
        {
            get
            {
                return this.Graph.AllGraphItems.OfType<InstancesReference>().Where(p => p.SourceIdentifier == this.Identifier);
            }
        }

        public IEnumerable<ITypedItem> AllProperties
        {
            get
            {
                foreach (var item in ComputedProperties) yield return item;
                foreach (var item in LocalProperties) yield return item;
            }
        }

        public IEnumerable<ITypedItem> BindableProperties
        {
            get
            {
                foreach (var item in ComputedProperties) yield return item;
                foreach (var item in LocalProperties) yield return item;
                foreach (var item in LocalCollections) yield return item;
                foreach (var item in LocalCommands) yield return item;

                var baseElement = BaseNode as ElementNode;
                if (baseElement != null)
                {
                    foreach (var item in baseElement.BindableProperties)
                    {
                        yield return item;
                    }
                }
            }
        }

        public IEnumerable<ITypedItem> AllPropertiesWithInherited
        {
            get
            {
                var baseElement = BaseNode as ElementNode;
                if (baseElement != null)
                {
                    foreach (var property in baseElement.AllProperties)
                    {
                        yield return property;
                    }
                    foreach (var property in baseElement.AllPropertiesWithInherited)
                    {
                        yield return property;
                    }
                }
            }
        }

        public virtual IEnumerable<PropertiesChildItem> InheritedProperties
        {
            get
            {
                var baseElement = BaseNode as ElementNode;
                if (baseElement != null)
                {
                    foreach (var property in baseElement.LocalProperties)
                    {
                        yield return property;
                    }
                    foreach (var property in baseElement.InheritedProperties)
                    {
                        yield return property;
                    }
                }
            }
        }

        public virtual IEnumerable<CollectionsChildItem> InheritedCollections
        {
            get
            {
                var baseElement = BaseNode as ElementNode;
                if (baseElement != null)
                {
                    foreach (var property in baseElement.LocalCollections)
                    {
                        yield return property;
                    }
                    foreach (var property in baseElement.InheritedCollections)
                    {
                        yield return property;
                    }
                }
            }
        }

        public virtual IEnumerable<CommandsChildItem> InheritedCommands
        {
            get
            {
                var baseElement = BaseNode as ElementNode;
                if (baseElement != null)
                {
                    foreach (var property in baseElement.LocalCommands)
                    {
                        yield return property;
                    }
                    foreach (var property in baseElement.InheritedCommands)
                    {
                        yield return property;
                    }
                }
            }
        }

        public virtual IEnumerable<CommandsChildItem> InheritedCommandsWithLocal
        {
            get
            {
                foreach (var item in LocalCommands)
                    yield return item;
                var baseElement = BaseNode as ElementNode;
                if (baseElement != null)
                {
                    foreach (var property in baseElement.LocalCommands)
                    {
                        yield return property;
                    }
                    foreach (var property in baseElement.InheritedCommands)
                    {
                        yield return property;
                    }
                }
            }
        }

        [Section("Properties", SectionVisibility.Always, OrderIndex = 0, IsNewRow = true)]
        public override IEnumerable<PropertiesChildItem> Properties
        {
            get
            {
                if (Graph.CurrentFilter == this)
                {
                    foreach (var item in InheritedProperties)
                    {
                        yield return item;
                    }
                }
                foreach (var item in LocalProperties)
                {
                    yield return item;
                }
            }
        }

        [Section("Collections", SectionVisibility.Always, OrderIndex = 1, IsNewRow = true)]
        public override IEnumerable<CollectionsChildItem> Collections
        {
            get
            {
                if (Graph.CurrentFilter == this)
                {
                    foreach (var item in InheritedCollections)
                    {
                        yield return item;
                    }
                }
                foreach (var item in LocalCollections)
                {
                    yield return item;
                }
            }
        }

        [Section("Commands", SectionVisibility.Always, OrderIndex = 3, IsNewRow = true)]
        public override IEnumerable<CommandsChildItem> Commands
        {
            get
            {
                if (Graph.CurrentFilter == this)
                {
                    foreach (var item in InheritedCommands)
                    {
                        yield return item;
                    }
                }
                foreach (var item in LocalCommands)
                {
                    yield return item;
                }
            }
        }

        public IEnumerable<ITypedItem> AllCommandHandlers
        {
            get
            {
                foreach (var item in LocalCommands)
                {
                    yield return item;
                }
            }
        }

        public IEnumerable<PropertiesChildItem> LocalProperties
        {
            get
            {
                return this.PersistedItems.OfType<PropertiesChildItem>();
            }
        }

        public IEnumerable<CollectionsChildItem> LocalCollections
        {
            get
            {
                return this.PersistedItems.OfType<CollectionsChildItem>();
            }
        }


        public IEnumerable<CommandsChildItem> LocalCommands
        {
            get
            {
                return this.PersistedItems.OfType<CommandsChildItem>();
            }
        }

        public override string TypeName
        {
            get { return this.Name.AsViewModel(); }
        }

        public override string ClassName
        {
            get { return this.Name.AsViewModel(); }
        }
    }

    public partial interface IElementConnectable : IDiagramNodeItem, IConnectable
    {
    }
}
