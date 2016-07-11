using uFrame.Editor.Core;

namespace uFrame.Editor.NavigationSystem
{
    public class NavigateByNameCommand : Command
    {
        public string FilterId { get; set; }
        public string ItemName { get; set; }
    }
}