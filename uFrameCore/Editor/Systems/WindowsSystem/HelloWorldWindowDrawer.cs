using uFrame.Editor.GraphUI.Drawers;
using uFrame.Editor.Platform;
using UnityEngine;

namespace uFrame.Editor.Windows
{
    public class HelloWorldWindowDrawer : Drawer<HelloWorldWindowViewModel>
    {

        public HelloWorldWindowDrawer(HelloWorldWindowViewModel viewModelObject) : base(viewModelObject)
        {
        }

        public override void Draw(IPlatformDrawer platform, float scale)
        {
            base.Draw(platform, scale);
            platform.DrawLabel(new Rect(0,0,100,100),ViewModel.Message,CachedStyles.GraphTitleLabel,DrawingAlignment.MiddleCenter);
        }
    }
}