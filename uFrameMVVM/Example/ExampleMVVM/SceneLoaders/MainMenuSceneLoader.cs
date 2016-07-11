namespace Example {
    using Example;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using uFrame.IOC;
    using uFrame.Kernel;
    using uFrame.Kernel.Serialization;
    using uFrame.MVVM;
    using UnityEngine;
    
    
    public class MainMenuSceneLoader : MainMenuSceneLoaderBase {
        
        protected override IEnumerator LoadScene(MainMenuScene scene, Action<float, string> progressDelegate) {
            yield break;
        }
        
        protected override IEnumerator UnloadScene(MainMenuScene scene, Action<float, string> progressDelegate) {
            yield break;
        }
    }
}
