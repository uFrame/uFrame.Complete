using uFrame.Editor.QuickAccess;
using uFrame.Editor.Windows;
using uFrame.IOC;

namespace uFrame.Editor.Unity
{
    public class UnityWindows : DiagramPlugin
    {
        public override bool Required
        {
            get { return true; }
        }

        public override void Initialize(UFrameContainer container)
        {
            base.Initialize(container);
            container.RegisterWindow<QuickAccessWindowViewModel>("QuickAccessWindowFactory")
                .HasPanel<QuickAccessWindowSearchPanel, QuickAccessWindowViewModel>()
                .WithDefaultInstance(_ => new QuickAccessWindowViewModel(new QuickAccessContext()
                {
                    ContextType = typeof(IInsertQuickAccessContext)
                }));

            container.RegisterWindow<QuickAccessWindowViewModel>("ConnectionWindowFactory")
                .HasPanel<QuickAccessWindowSearchPanel, QuickAccessWindowViewModel>()
                .WithDefaultInstance(_ => new QuickAccessWindowViewModel(new QuickAccessContext()
                {
                    ContextType = typeof(IConnectionQuickAccessContext)
                }));
        }
    }
}