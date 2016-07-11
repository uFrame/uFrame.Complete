using uFrame.Editor.Platform;
using UnityEngine;

namespace uFrame.Editor.Wizards.Events
{
    public interface IDrawWorkspacesWizard
    {
        void DrawWorkspacesWizard(IPlatformDrawer platform, Rect bounds);
    }
}
