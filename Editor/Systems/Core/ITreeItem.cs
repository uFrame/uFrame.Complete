using System.Collections.Generic;

namespace uFrame.Editor.Core
{
    public interface ITreeItem : IItem
    {
        IItem ParentItem { get; }
        IEnumerable<IItem> Children { get; }
        bool Expanded { get; set; }
    }
}