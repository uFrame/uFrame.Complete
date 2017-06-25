using System;
using System.ComponentModel;
using uFrame.Kernel;

namespace uFrame.Editor.GraphUI.ViewModels
{
    public static class ViewModelExtensions
    {
        public static Action SubscribeToProperty<TViewModel>(this TViewModel vm, string propertyName, Action<TViewModel> action) where TViewModel : ViewModel
        {
            PropertyChangedSimpleEventHandler handler = (sender, args) =>
            {
                action(sender as TViewModel);
            };;
            vm.PropertyChanged += handler;

            return ()=> { vm.PropertyChanged -= handler; };
        }
    }
}