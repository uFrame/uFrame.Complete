namespace Example {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    
    
    public class SubScreenController : SubScreenControllerBase {
        
        public override void InitializeSubScreen(SubScreenViewModel viewModel) {
            base.InitializeSubScreen(viewModel);
            // This is called when a SubScreenViewModel is created
        }
        
        public override void Close(SubScreenViewModel viewModel) {
            base.Close(viewModel);

            Publish(new RequestMainMenuScreenCommand()
            {
                ScreenType = typeof(MenuScreenViewModel)
            });
        }
    }
}
