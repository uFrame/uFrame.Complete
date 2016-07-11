using UnityEngine.UI;

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
    
    
    public class MenuScreenView : MenuScreenViewBase {

        public Button LevelSelectButton;
        public Button SettingsButton;
        public Button ExitButton;

        protected override void InitializeViewModel(uFrame.MVVM.ViewModels.ViewModel model) {
            base.InitializeViewModel(model);
            // NOTE: this method is only invoked if the 'Initialize ViewModel' is checked in the inspector.
            // var vm = model as MenuScreenViewModel;
            // This method is invoked when applying the data from the inspector to the viewmodel.  Add any view-specific customizations here.
        }
        
        public override void Bind() {
            base.Bind();
            // Use this.MenuScreen to access the viewmodel.
            // Use this method to subscribe to the view-model.
            // Any designer bindings are created in the base implementation.

            // Bind each button to handler:
            // When button is clicked, handler is excuted
            // Ex: When we press LevelSelectButton, we publish
            // RequestMainMenuScreenCommand and pass LevelSelectScreenViewModel type
            this.BindButtonToHandler(LevelSelectButton, () =>
            {
                Publish(new RequestMainMenuScreenCommand()
                {
                    ScreenType = typeof(LevelSelectScreenViewModel)
                });
            });

            this.BindButtonToHandler(SettingsButton, () =>
            {
                Publish(new RequestMainMenuScreenCommand()
                {
                    ScreenType = typeof(SettingScreenViewModel)
                });
            });

            // This follows the same logic, but we use Method Group syntax.
            // And we do not publish event. We just quit.
            this.BindButtonToHandler(ExitButton, Application.Quit);
            //Equivalent to 
            //this.BindButtonToHandler(ExitButton, () => { Application.Quit; });
        }
    }
}
