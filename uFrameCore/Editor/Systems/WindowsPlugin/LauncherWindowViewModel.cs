using System.Collections.Generic;
using uFrame.Editor.Windows;

namespace uFrame.Editor.WindowsPlugin
{
    public class LauncherWindowViewModel
    {

        public List<IWindowFactory> AvailableWindows
        {
            get { return WindowsPlugin.LaucherWindows; }
            set { }
        }

    }
}