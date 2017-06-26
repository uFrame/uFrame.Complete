using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace uFrame.MVVM.ViewModels
{
    /// <summary>
    /// The view model manager is a class that encapsulates a list of viewmodels
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ViewModelManager<T> : IViewModelManager<T> where T : ViewModel
    {
        private readonly List<T> _viewModels = new List<T>();

        public IList<T> ViewModels
        {
            get { return _viewModels; }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<T>) this).GetEnumerator();
        }

        public void Add(ViewModel viewModel)
        {
            Add((T) viewModel);
        }

        public void Remove(ViewModel viewModel)
        {
            Remove((T) viewModel);
        }

        public void Add(T viewModel) {
            if (!ViewModels.Contains(viewModel))
                ViewModels.Add(viewModel);
        }

        public void Remove(T viewModel) {
            ViewModels.Remove(viewModel);
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator() {
            return ViewModels.GetEnumerator();
        }
    }
}