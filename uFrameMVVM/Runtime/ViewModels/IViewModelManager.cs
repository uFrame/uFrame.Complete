using System.Collections;
using System.Collections.Generic;

namespace uFrame.MVVM.ViewModels
{
    /// <summary>
    /// The view model manager is a class that encapsulates a list of viewmodels
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IViewModelManager : IEnumerable
    {
        void Add(ViewModel viewModel);
        void Remove(ViewModel viewModel);
    }

    /// <summary>
    /// The view model manager is a class that encapsulates a list of viewmodels
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IViewModelManager<T> : IViewModelManager, IEnumerable<T> where T : ViewModel
    {
        void Add(T viewModel);
        void Remove(T viewModel);
        IList<T> ViewModels { get; }
    }
}