namespace Example {
    using Example;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    
    
    public class LevelScene : LevelSceneBase {
        /// <summary>
        /// An ID to be used to get meta information for this level 
        /// </summary>
        public int Id;

        private LevelRootViewModel _levelRoot;

        /// <summary>
        /// When requested, finds LevelRootView in the hierarchy, extracts the viewmodel and caches it
        /// </summary>
        public LevelRootViewModel LevelRoot
        {
            get { return _levelRoot ?? (_levelRoot = GetComponentInChildren<LevelRootViewBase>().LevelRoot); }
            private set { _levelRoot = value; }
        }
    }
}
