using uFrame.IOC;

namespace Example
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;


    public class LoginScreenController : LoginScreenControllerBase
    {
        [Inject]
        public UserManagementService UserManagementService;

        public override void InitializeLoginScreen(LoginScreenViewModel viewModel)
        {
            base.InitializeLoginScreen(viewModel);
            // This is called when a LoginScreenViewModel is created
        }

        public override void Login(LoginScreenViewModel viewModel)
        {
            base.Login(viewModel);
            UserManagementService.AuthorizeLocalUser(viewModel.Username, viewModel.Password);
        }
    }
}
