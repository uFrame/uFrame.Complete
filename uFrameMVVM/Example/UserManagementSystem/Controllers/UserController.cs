namespace Example {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    
    
    public class UserController : UserControllerBase {
        
        public override void InitializeUser(UserViewModel viewModel) {
            base.InitializeUser(viewModel);
            // This is called when a UserViewModel is created
        }
    }
}
