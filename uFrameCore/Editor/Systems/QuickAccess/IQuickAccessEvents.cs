using System.Collections.Generic;
using uFrame.Editor.Core;

namespace uFrame.Editor.QuickAccess
{
    public interface IQuickAccessEvents
    {
        void QuickAccessItemsEvents(QuickAccessContext context, List<IItem> items);
    }
}