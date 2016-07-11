using System.ComponentModel;
using uFrame.Editor.Core;

namespace uFrame.Editor.Koinonia.Commands
{
    public class LoginCommand : IBackgroundCommand
    {
        public string Title
        {
            get { return "Login to Invert Empire"; }
            set
            {
                
            }
        }

        public string Username { get; set; }
        public string Password { get; set; }

        public BackgroundWorker Worker { get; set; }
    }




}
