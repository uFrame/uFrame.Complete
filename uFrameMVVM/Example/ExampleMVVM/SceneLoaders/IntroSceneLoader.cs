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
    
    
    public class IntroSceneLoader : IntroSceneLoaderBase {
        
        protected override IEnumerator LoadScene(IntroScene scene, Action<float, string> progressDelegate) {
            yield break;
        }
        
        protected override IEnumerator UnloadScene(IntroScene scene, Action<float, string> progressDelegate) {
            yield break;
        }
    }
}
