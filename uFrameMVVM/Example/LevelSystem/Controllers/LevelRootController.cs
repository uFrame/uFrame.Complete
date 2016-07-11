using uFrame.Kernel;

namespace Example {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    
    
    public class LevelRootController : LevelRootControllerBase {
        
        public override void InitializeLevelRoot(LevelRootViewModel viewModel) {
            base.InitializeLevelRoot(viewModel);
            // This is called when a LevelRootViewModel is created
        }

        public override void FinishCurrentLevel(LevelRootViewModel viewModel)
        {
            base.FinishCurrentLevel(viewModel);

            //Simple scene transition.

            Publish(new UnloadSceneCommand()
            {
                SceneName = viewModel.CurrentLevel.LevelScene
            });

            Publish(new LoadSceneCommand()
            {
                SceneName = "MainMenuScene"
            });
        }
    }
}
