using System.Collections.Generic;
using uFrame.Editor.Platform;
using uFrame.Editor.Wizards.Data;
using UnityEngine;

namespace uFrame.Editor.Wizards.Events
{
    public interface IDrawWorkspacesList
    {
        void DrawWorkspacesList(IPlatformDrawer platform, Rect bounds, List<WorkspacesListItem> items );
    }
}
