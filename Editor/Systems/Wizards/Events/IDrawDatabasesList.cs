using System.Collections.Generic;
using uFrame.Editor.Platform;
using uFrame.Editor.Wizards.Data;
using UnityEngine;

namespace uFrame.Editor.Wizards.Events
{
    public interface IDrawDatabasesList
    {
        void DrawDatabasesList(IPlatformDrawer Drawer, Rect bounds, List<DatabasesListItem> items);
    }
}
