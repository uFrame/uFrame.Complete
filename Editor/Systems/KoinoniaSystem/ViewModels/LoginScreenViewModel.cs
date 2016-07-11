using uFrame.Editor.Core;
using uFrame.Editor.Koinonia.Commands;
using uFrame.Editor.Windows;

namespace uFrame.Editor.Koinonia.ViewModels
{
    public class LoginScreenViewModel : WindowViewModel
    {

        public string Username { get; set; }
        public string Password { get; set; }

    

        public void Login()
        {
            InvertApplication.ExecuteInBackground(new LoginCommand()
            {
                Password = Password,
                Username = Username
            });
        }

    }
}
