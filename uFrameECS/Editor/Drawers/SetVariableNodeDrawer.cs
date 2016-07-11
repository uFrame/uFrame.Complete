using uFrame.ECS.Editor;
using uFrame.Editor.DebugSystem;
using uFrame.Editor.GraphUI.Drawers;
using uFrame.Editor.Platform;
using UnityEditor;
using UnityEngine;

namespace uFrame.ECS.Editor
{
    public class SetVariableNodeDrawer : GenericNodeDrawer<SetVariableNode,SetVariableNodeViewModel> {
        
        public SetVariableNodeDrawer(SetVariableNodeViewModel viewModel) : 
                base(viewModel) {
        }
        public override void Draw(IPlatformDrawer platform, float scale)
        {
            base.Draw(platform, scale);
            if (EditorApplication.isPaused)
            {
                if (NodeViewModel.GraphItem.Identifier != DebugSystem.CurrentBreakId)
                {
                    var adjustedBounds = new Rect(Bounds.x - 9, Bounds.y + 1, Bounds.width + 19, Bounds.height + 9);
                    platform.DrawStretchBox(adjustedBounds, CachedStyles.BoxHighlighter5, 20);
                }
                else
                {
                    var adjustedBounds = new Rect(Bounds.x - 9, Bounds.y + 1, Bounds.width + 19, Bounds.height + 9);
                    platform.DrawStretchBox(adjustedBounds, CachedStyles.BoxHighlighter3, 20);
                }
            }

        }
    }
}
