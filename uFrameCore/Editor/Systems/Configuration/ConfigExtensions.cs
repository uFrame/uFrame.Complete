using System;
using uFrame.Editor.Graphs.Data;
using uFrame.IOC;

namespace uFrame.Editor.Configurations
{
    public static class ConfigExtensions
    {
        public static NodeConfig<TNode> GetNodeConfig<TNode>(this IUFrameContainer container) where TNode : GenericNode, IConnectable
        {
            var config = GetNodeConfig(container, typeof(TNode)) as NodeConfig<TNode>;

            return config;
        }

        public static NodeConfigBase GetNodeConfig(this IUFrameContainer container, Type nodeType)
        {
            
            var config = container.Resolve<NodeConfigBase>(nodeType.Name);    
            if (config == null)
            {
                var nodeconfigType = typeof (NodeConfig<>).MakeGenericType(nodeType);
                var nodeConfig = Activator.CreateInstance(nodeconfigType,container) as NodeConfigBase;
                nodeConfig.NodeType = nodeType;
                container.RegisterInstance<NodeConfigBase>(nodeConfig, nodeType.Name);
                //nodeConfig.Section(string.Empty, _ => _.PersistedItems.OfType<GenericConnectionReference>(), false);
                return nodeConfig;
            }
            return config;
        }

        //public static IUFrameContainer ScaffoldNodeChild<TNode, TChildItem>(this IUFrameContainer container, string header = null)
        //    where TChildItem : GenericNodeChildItem
        //    where TNode : GenericNode, IConnectable
        //{
        //    container.RegisterNodeSection<TNode, TChildItem>(header);
        //    container.RegisterGraphItem<TChildItem, ScaffoldNodeChildItem<TChildItem>.ViewModel, ScaffoldNodeChildItem<TChildItem>.Drawer>();
        //    return container;
        //}

        //public static IUFrameContainer RegisterNodeSection<TNode, TChildItem>(this IUFrameContainer container, string header = null, Func<TNode,IEnumerable<TChildItem>> selector = null) where TNode : GenericNode, IConnectable
        //{
        //    var config = container.GetNodeConfig<TNode>();
        //    var sectionConfig = new NodeConfigSection<TNode>()
        //    {
        //        ChildType = typeof (TChildItem),
        //        Name = header,
        //    };
        //    if (selector != null)
        //    {
        //        sectionConfig.Selector = p => selector(p).Cast<IGraphItem>();
        //    }
        //    config.Sections.Add(sectionConfig);
        //    return container;
        //}




    }
}