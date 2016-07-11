using uFrame.Editor.Platform;
using UnityEngine;

namespace uFrame.Editor.GraphUI.Drawers
{
    public interface IInspectorPropertyDrawer
    {
        void Refresh(IPlatformDrawer platform, Vector2 position, PropertyFieldDrawer viewModel);
        void Draw(IPlatformDrawer platform, float scale, PropertyFieldDrawer viewModel);
    }
}