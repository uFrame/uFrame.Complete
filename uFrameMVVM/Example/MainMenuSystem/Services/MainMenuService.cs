using uFrame.Kernel.Collection;

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


    public class MainMenuService : MainMenuServiceBase
    {

        //Inject MainMenuRoot view model with id "MainMenuRoot"
        [Inject("MainMenuRoot")]
        public MainMenuRootViewModel MainMenuRoot;
        [Inject("LocalUser")]
        public UserViewModel LocalUser;

        /// <summary>
        /// This method is invoked whenever the kernel is loading
        /// Since the kernel lives throughout the entire lifecycle  of the game, this will only be invoked once.
        /// </summary>
        public override void Setup()
        {
            base.Setup();
            // Use the line below to subscribe to events
            // this.OnEvent<MyEvent>().Subscribe(myEventInstance => { TODO });

            //Every time CurrentScreenType changes, invoke ChangeMainMenuScreen and pass it a new value
            MainMenuRoot.CurrentScreenTypeProperty.Subscribe(ChangeMainMenuScreen);

            //Every time new Screen is added to the Screens collection, invoke ScreenAdded and pass the new screen
            MainMenuRoot.Screens
                .Where(_ => _.Action == NotifyCollectionChangedAction.Add)
                .Select(_ => _.NewItems[0] as SubScreenViewModel)
                .Subscribe(ScreenAdded);

            //Every time user athorization state changes activate corresponding screen
            LocalUser.authStateProperty
                .StartWith(LocalUser.authState)
                //Force subscribtion to be triggered immediately with the current value
                .Subscribe(OnAuthorizationStateChanged);
        }

        private void OnAuthorizationStateChanged(AuthState state)
        {
            //Just activate right screen based on authorization state`
            switch (state)
            {
                case AuthState.Authorized:
                    //Show menu if user is authorized
                    MainMenuRoot.CurrentScreenType = typeof(MenuScreenViewModel);
                    break;
                case AuthState.Unauthorized:
                    //Show login screen if user is unauthorized
                    MainMenuRoot.CurrentScreenType = typeof(LoginScreenViewModel);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("state", state, null);
            }
        }

        private void ScreenAdded(SubScreenViewModel screen)
        {
            //if screen is of current type, activate it; else deactivate it
            screen.IsActive = MainMenuRoot.CurrentScreenType == screen.GetType();
        }

        private void ChangeMainMenuScreen(Type screenType)
        {

            Debug.Log(string.Format("Screen type changed to {0}", screenType == null ? "null" : screenType.Name));

            //Cast to IEnumerable to avoid ambiguosity between UniRX and Collections namespaces
            var screens = MainMenuRoot.Screens as IEnumerable<SubScreenViewModel>;

            //Find screen we want to activate
            var screen = screens.FirstOrDefault(s => s.GetType() == screenType);

            //Deactivate all the screens except the one we need
            screens.Where(s => s.GetType() != screenType).ToList().ForEach(s => s.IsActive = false);

            //If screen of matching type is found - activate is
            if (screen != null) screen.IsActive = true;
        }

        /// <sumarry>
        // This method is executed when using this.Publish(new RequestMainMenuScreenCommand())
        /// </sumarry>
        public override void RequestMainMenuScreenCommandHandler(RequestMainMenuScreenCommand data)
        {
            base.RequestMainMenuScreenCommandHandler(data);
            // Process the commands information. Also, you can publish new events by using the line below.
            // this.Publish(new AnotherEvent())

            //Change screen to what was requested
            MainMenuRoot.CurrentScreenType = data.ScreenType;
        }
    }
}
