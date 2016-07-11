using uFrame.Editor.Core;

namespace uFrame.Editor.NavigationSystem
{
    public class NavigateByIdCommand : Command
    {
        public string Identifier { get; set; }
        public string FilterId { get; set; }
    }
}