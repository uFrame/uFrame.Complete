using System;

namespace uFrame.Editor.Core
{
    public class LambdaCommand :ICommand
    {
        public LambdaCommand(string title,Action action)
        {
            Title = title;
            Action = action;
        }
        public string Title { get; set; }
        public Action Action { get; set; }
    }
}