namespace Example {
    using Example;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using uFrame.IOC;
    using uFrame.Kernel;
    using uFrame.MVVM;
    using UniRx;
    using UnityEngine;

    /*
     * This service introduces example of interesting database.
     * Check the kernel how LevelDescriptors live on a certain GameObject and this service reads them.
     */
    
    public class LevelManagementService : LevelManagementServiceBase 
    {
        private List<LevelDescriptor> _levels;

        //Game object holding LevelDescriptor components 
        public GameObject LevelsContainer;

        // This list will hold all the registered levels
        // You can add level descriptors dynamically by adding new LevelDescriptor
        // component on the service object and calling UpdateLevels
        public List<LevelDescriptor> Levels
        {
            get { return _levels ?? (_levels = new List<LevelDescriptor>()); }
            set { _levels = value; }
        }
        
        /// <summary>
        /// This method is invoked whenever the kernel is loading
        /// Since the kernel lives throughout the entire lifecycle  of the game, this will only be invoked once.
        /// </summary>
        public override void Setup() {
            base.Setup();
            // Use the line below to subscribe to events
            // this.OnEvent<MyEvent>().Subscribe(myEventInstance => { TODO });

            //On setup register levels initially
            UpdateLevels();
        }

        private void UpdateLevels()
        {
            var levelDescriptorComponents = LevelsContainer.GetComponents<LevelDescriptor>().Except(Levels);
            //Get all non registered level descriptors
            Levels.AddRange(levelDescriptorComponents); //Add those to the list of registered levels
        }
    }
}
