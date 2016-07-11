namespace uFrame.MVVM
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using uFrame.Editor.GraphUI.Drawers;
    using uFrame.Editor.Platform;
    using UnityEngine;

    public class StateNodeDrawer : GenericNodeDrawer<StateNode, StateNodeViewModel>
    {

        public StateNodeDrawer(StateNodeViewModel viewModel) :
            base(viewModel)
        {
        }

        public override void Draw(IPlatformDrawer platform, float scale)
        {
            base.Draw(platform, scale);
            if (NodeViewModel.IsCurrentState)
            {
                Rect r = new Rect(this.Bounds.x - 9f, this.Bounds.y + 1f, this.Bounds.width + 19f, this.Bounds.height + 9f);
                platform.DrawStretchBox(r.Scale(base.Scale), CachedStyles.BoxHighlighter1, 20f);
            }
        }
    }
}
