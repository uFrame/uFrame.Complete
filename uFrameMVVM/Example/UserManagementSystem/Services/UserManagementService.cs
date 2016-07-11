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


    public class UserManagementService : UserManagementServiceBase
    {
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

            LocalUser.authState = AuthState.Unauthorized;
        }

        public void AuthorizeLocalUser(string Username, string Password)
        {
            if (Username == "uframe" && Password == "uframe")
            {
                Debug.Log("authorized in service");
                LocalUser.authState = AuthState.Authorized;
            }
        }
    }
}
