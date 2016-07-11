using uFrame.Editor.Core;
using uFrame.Editor.Database.Data;
using uFrame.Editor.Windows;

namespace uFrame.Editor.Koinonia.ViewModels
{
    public class PackageManagerWindow : WindowViewModel
    {
        private IRepository _repository;

        public IRepository Repository
        {
            get { return _repository ?? (_repository = InvertApplication.Container.Resolve<IRepository>()); }
            set { _repository = value; }
        }

    }
}
