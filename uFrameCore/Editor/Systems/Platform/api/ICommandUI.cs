using uFrame.Editor.Core;

namespace uFrame.Editor.Platform
{
    public interface ICommandUI
    {
        void AddCommand(ICommand command);
        void Go();
        
        void GoBottom();
    }
}