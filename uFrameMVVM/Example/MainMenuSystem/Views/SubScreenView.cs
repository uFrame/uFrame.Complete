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
    
    
    public class SubScreenView : SubScreenViewBase 
    {
        public GameObject ScreenUIContainer;

        protected override void InitializeViewModel(uFrame.MVVM.ViewModels.ViewModel model) {
            base.InitializeViewModel(model);
            // NOTE: this method is only invoked if the 'Initialize ViewModel' is checked in the inspector.
            // var vm = model as SubScreenViewModel;
            // This method is invoked when applying the data from the inspector to the viewmodel.  Add any view-specific customizations here.
        }
        
        public override void Bind() {
            base.Bind();
            // Use this.SubScreen to access the viewmodel.
            // Use this method to subscribe to the view-model.
            // Any designer bindings are created in the base implementation.
        }

        public override void IsActiveChanged(Boolean active)
        {

            /* 
             * Always make sure, that you cache components used in the binding handlers BEFORE you actually bind.
             * This is important, since when Binding to the viewmodel, Handlers are invoked immidiately
             * with the current values, to ensure that view state is consistant.
             * For example, you can do this in Awake or in KernelLoading/KernelLoaded.
             * However, in this example we simply use public property to get a reference to ScreenUIContainer.
             * So we do not have to cache anything.
             */
            ScreenUIContainer.gameObject.SetActive(active);

        }
    }
}
