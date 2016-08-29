using uFrame.IOC;

namespace Example {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    
    
    public class SettingScreenController : SettingScreenControllerBase {

        [Inject]
        public SettingService SettingService;

        public override void InitializeSettingScreen(SettingScreenViewModel viewModel) {
            base.InitializeSettingScreen(viewModel);
            // This is called when a SettingScreenViewModel is created
            /* Add known resolutions to the list */
            viewModel.AvailableResolutions.AddRange(SettingService.AvailableResolutions);

            /* Setup current resolution */
            viewModel.Resolution = SettingService.Resolution;

            /* Setup volume */
            viewModel.Volume = SettingService.Volume;
        }
        
        public override void Default(SettingScreenViewModel viewModel) {
            base.Default(viewModel);
        }
        
        public override void Apply(SettingScreenViewModel viewModel) {
            base.Apply(viewModel);

            /* Pass selected values to the service */
            SettingService.Resolution = viewModel.Resolution;
            SettingService.Volume = viewModel.Volume;
        }
    }
}
