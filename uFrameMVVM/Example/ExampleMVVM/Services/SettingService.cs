namespace Example
{
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


    public class SettingService : SettingServiceBase
    {
        //Simple property which works of PlayerPrefs
        public float Volume
        {
            get { return PlayerPrefs.GetFloat("Settings_Volume", 0.5f); }
            set { PlayerPrefs.SetFloat("Settings_Volume", value); }
        }

        //Advanced version of it, which works of PlayerPrefs, but uses string to 
        //store serialized version of the object
        //Please notice that by default, Simple Class does not have Serialize/Deserialize methods
        //This is generated due to the extension of SimpleClassTemplate.
        public ResolutionInformation Resolution
        {
            get
            {
                if (PlayerPrefs.HasKey("Settings_Resolution"))
                {
                    var resInfo = new ResolutionInformation();
                    //Deserialize into the new instance
                    resInfo.Deserialize(PlayerPrefs.GetString("Settings_Resolution"));
                    return resInfo;
                }
                else
                {
                    //return default value, if player never changed settings
                    return AvailableResolutions.First();
                }
            }
            set
            {
                //Serialize
                PlayerPrefs.SetString("Settings_Resolution", value.Serialize());
            }
        }

        private IEnumerable<ResolutionInformation> _availableResolutions;

        public IEnumerable<ResolutionInformation> AvailableResolutions
        {
            get { return _availableResolutions ?? (_availableResolutions = GetAvailableResolutions()); }
            set { _availableResolutions = value; }
        }

        protected IEnumerable<ResolutionInformation> GetAvailableResolutions()
        {
            yield return new ResolutionInformation() { Width = 800, Height = 600 };
            yield return new ResolutionInformation() { Width = 1200, Height = 800 };
            yield return new ResolutionInformation() { Width = 1600, Height = 1200 };
        }

        private void OnApplicationQuit()
        {
            PlayerPrefs.Save();
        }

        /// <summary>
        /// This method is invoked whenever the kernel is loading
        /// Since the kernel lives throughout the entire lifecycle  of the game, this will only be invoked once.
        /// </summary>
        public override void Setup()
        {
            base.Setup();
            // Use the line below to subscribe to events
            // this.OnEvent<MyEvent>().Subscribe(myEventInstance => { TODO });
        }
    }
}
