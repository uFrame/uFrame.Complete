using System;
using uFrame.Editor.Platform;
using uFrame.Editor.Wizards.Data;
using UnityEngine;

namespace uFrame.Editor.Wizards.Events
{
    public interface IDrawActionDialog
    {
        void DrawActionDialog(IPlatformDrawer platform, Rect bounds, ActionItem item, Action cancel = null);
    }
}