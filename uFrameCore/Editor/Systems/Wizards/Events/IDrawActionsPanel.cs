using System;
using System.Collections.Generic;
using uFrame.Editor.Platform;
using uFrame.Editor.Wizards.Data;
using UnityEngine;

namespace uFrame.Editor.Wizards.Events
{
    public interface IDrawActionsPanel
    {
        void DrawActionsPanel(IPlatformDrawer platform, Rect bounds, List<ActionItem> actions, Action<ActionItem, Vector2> primaryAction,
            Action<ActionItem, Vector2> secondaryAction = null);
    }
}
