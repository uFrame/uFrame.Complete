using uFrame.Editor.Platform;
using UnityEngine;

namespace uFrame.Editor.Wizards.Events
{
    public interface IDrawDatabasesWizard
    {
        void DrawDatabasesWizard(IPlatformDrawer Drawer, Rect bounds);
    }
}
