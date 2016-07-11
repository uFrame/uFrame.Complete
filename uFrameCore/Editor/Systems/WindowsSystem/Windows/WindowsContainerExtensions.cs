using uFrame.IOC;

namespace uFrame.Editor.Windows
{
    public static class WindowsContainerExtensions
    {
        public static WindowFactory<TWindow> RegisterWindow<TWindow>(this IUFrameContainer container, string name) where TWindow : class, IWindow
        {
            var factory = new WindowFactory<TWindow>(container,name);
            container.RegisterInstance(factory,name);
            container.RegisterInstance<IWindowFactory>(factory,name);
            //container.RegisterInstance<IWindowFactory<TWindow>>(factory,name);
            return factory;
        }
        
    }
}