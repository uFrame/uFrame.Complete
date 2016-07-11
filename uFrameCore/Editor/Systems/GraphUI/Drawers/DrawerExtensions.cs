using uFrame.Editor.Configurations;
using uFrame.Editor.Graphs.Data;
using uFrame.Editor.GraphUI.Scaffolding;
using uFrame.Editor.GraphUI.ViewModels;
using uFrame.IOC;

namespace uFrame.Editor.GraphUI.Drawers
{
    public static class DrawerExtensions
    {
        public static IUFrameContainer RegisterGraphItem<TModel, TViewModel, TDrawer>(this IUFrameContainer container)
        {
            container.RegisterDataViewModel<TModel, TViewModel>();
            container.RegisterDrawer<TViewModel, TDrawer>();
            return container;
        }

        public static void RegisterItemDrawer<TViewModel, TDrawer>(this IUFrameContainer container)
        {
            container.RegisterRelation<TViewModel, IDrawer, TDrawer>();
        }
        public static void RegisterDrawer<TViewModel, TDrawer>(this IUFrameContainer container)
        {
            container.RegisterRelation<TViewModel, IDrawer, TDrawer>();
        }

        public static IUFrameContainer RegisterChildGraphItem<TModel, TViewModel, TDrawer>(this IUFrameContainer container)
        {
            container.RegisterRelation<TModel, ItemViewModel, TViewModel>();
            container.RegisterItemDrawer<TViewModel, TDrawer>();
            return container;
        }

        public static NodeConfig<TNodeData> AddNode<TNodeData, TNodeViewModel, TNodeDrawer>(this IUFrameContainer container, string name) where TNodeData : GenericNode, IConnectable
        {

            container.AddItem<TNodeData>();
            container.RegisterGraphItem<TNodeData, TNodeViewModel, TNodeDrawer>();
            var config = container.GetNodeConfig<TNodeData>();
            config.Name = name;
            return config;
        }
        public static NodeConfig<TNodeData> AddNode<TNodeData>(this IUFrameContainer container, string tag = null)
            where TNodeData : GenericNode
        {
            var config = container.AddNode<TNodeData, ScaffoldNode<TNodeData>.ViewModel, ScaffoldNode<TNodeData>.Drawer>(tag);
            return config;
        }


        public static IUFrameContainer AddItem<TNodeData, TNodeViewModel, TNodeDrawer>(this IUFrameContainer container) where TNodeData : IDiagramNodeItem
        {
            container.RegisterChildGraphItem<TNodeData, TNodeViewModel, TNodeDrawer>();
            return container;
        }
        public static IUFrameContainer AddItem<TNodeData>(this IUFrameContainer container) where TNodeData : IDiagramNodeItem
        {
            container.RegisterChildGraphItem<TNodeData, ScaffoldNodeChildItem<TNodeData>.ViewModel, ScaffoldNodeChildItem<TNodeData>.Drawer>();
            return container;
        }
        public static IUFrameContainer AddTypeItem<TNodeData>(this IUFrameContainer container) where TNodeData : ITypedItem
        {
            container.AddItem<TNodeData>();
            container.RegisterChildGraphItem<TNodeData,
                ScaffoldNodeTypedChildItem<TNodeData>.ViewModel,
                ScaffoldNodeTypedChildItem<TNodeData>.Drawer>();
            return container;
        }
        public static IUFrameContainer AddTypeItem<TNodeData, TViewModel, TDrawer>(this IUFrameContainer container) where TNodeData : ITypedItem
        {
            container.AddItem<TNodeData>();
            container.RegisterChildGraphItem<TNodeData,
                TViewModel,
                TDrawer>();
            return container;
        }
        public static NodeConfig<TGraphNode> AddGraph<TGraphType, TGraphNode>(this IUFrameContainer container, string name)
            where TGraphType : IGraphData
            where TGraphNode : GenericNode, new()
        {

            container.Register<IGraphData, TGraphType>(name);
            return AddNode<TGraphNode>(container, name);
        }
        public static IUFrameContainer RegisterGraphItem<TModel>(this UFrameContainer container) where TModel : GenericNode
        {
            container.RegisterGraphItem<TModel, ScaffoldNode<TModel>.ViewModel, ScaffoldNode<TModel>.Drawer>();
            //RegisterDrawer();
            return container;
        }
    }
}