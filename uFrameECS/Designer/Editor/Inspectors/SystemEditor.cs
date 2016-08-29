using uFrame.ECS.Systems;
using uFrame.Editor.Core;
using UnityEditor;

namespace uFrame.ECS.Editor
{
    [UnityEditor.CustomEditor(typeof(EcsSystem), true)]
    public class SystemEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            InvertApplication.SignalEvent<IDrawUnityInspector>(_ => _.DrawInspector(target));
        }
    }
}
