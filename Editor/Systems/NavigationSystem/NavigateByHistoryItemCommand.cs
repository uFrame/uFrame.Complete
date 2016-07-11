using uFrame.Editor.Core;

namespace uFrame.Editor.NavigationSystem
{
    public class NavigateByHistoryItemCommand : Command
    {
        public NavHistoryItem Item { get; set; }
    }
}