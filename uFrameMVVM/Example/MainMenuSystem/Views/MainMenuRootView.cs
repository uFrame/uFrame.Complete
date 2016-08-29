namespace Example {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using uFrame.Kernel;
    using uFrame.Kernel.Serialization;
    using uFrame.MVVM;
    using uFrame.MVVM.Bindings;
    using uFrame.MVVM.Services;
    using uFrame.MVVM.ViewModels;
    using UniRx;
    using UnityEngine;
    
    
    public class MainMenuRootView : MainMenuRootViewBase {
        
        public override uFrame.MVVM.Views.ViewBase ScreensCreateView(uFrame.MVVM.ViewModels.ViewModel viewModel) {
            return InstantiateView(viewModel);
        }
        
        public override void ScreensAdded(uFrame.MVVM.Views.ViewBase view) {
        }
        
        public override void ScreensRemoved(uFrame.MVVM.Views.ViewBase view) {
        }
        
        protected override void InitializeViewModel(uFrame.MVVM.ViewModels.ViewModel model) {
            base.InitializeViewModel(model);
            // NOTE: this method is only invoked if the 'Initialize ViewModel' is checked in the inspector.
            // var vm = model as MainMenuRootViewModel;
            // This method is invoked when applying the data from the inspector to the viewmodel.  Add any view-specific customizations here.
        }
        
        public override void Bind() {
            base.Bind();
            // Use this.MainMenuRoot to access the viewmodel.
            // Use this method to subscribe to the view-model.
            // Any designer bindings are created in the base implementation.
        }
    }
}
