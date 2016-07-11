using uFrame.Editor.Core;
using uFrame.Editor.Menus;

namespace uFrame.Editor.Platform
{
    public class ToolbarItem
    {
        public ICommand Command { get; set; }
        public ToolbarPosition Position { get; set; }
        public int Order { get; set; }
        public bool IsDropdown { get; set; }
        public bool Checked { get; set; }
        public bool IsDelayCall { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}