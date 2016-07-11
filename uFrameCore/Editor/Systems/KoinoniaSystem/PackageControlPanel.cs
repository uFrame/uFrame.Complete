using uFrame.Editor.Koinonia.Data;
using UnityEditor;

namespace uFrame.Editor.Koinonia
{
    public class PackageControlPanel : EditorWindow
    {

        public UFramePackageDescriptor Package { get; set; }

        void OnGUI()
        {
            //if(Package!=null)
            //InvertApplication.SignalEvent<IDrawPackageControlPanel>(_ => _.DrawControlPanel(new Rect(0, 0, Screen.width, Screen.height),Package));
        }

        void Update()
        {
            Repaint();
        }

    }
}