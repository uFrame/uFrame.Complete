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
    
    
    public class AssetsLoadingSceneLoader : AssetsLoadingSceneLoaderBase {
        
        protected override IEnumerator LoadScene(AssetsLoadingScene scene, Action<float, string> progressDelegate) {
            //We publish this event, so AssetsLoadingService can handle it and start loading assets.
            Publish(new StartAssetLoadingCommand());
            yield break;
        }
        
        protected override IEnumerator UnloadScene(AssetsLoadingScene scene, Action<float, string> progressDelegate) {
            yield break;
        }
    }
}
