using uFrame.Kernel;
using UnityEditor;

namespace uFrame.Editor.Inspectors
{
    [CustomEditor(typeof(Scene), true)]
    public class SceneInspector : UnityEditor.Editor
    {
        public Scene Target
        {
            get { return target as Scene; }
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (!string.IsNullOrEmpty(Target.DefaultKernelScene))
            {
                EditorGUILayout.HelpBox(string.Format("Leave the 'Kernel Scene' property blank to use the default '{0}'", Target.DefaultKernelScene), MessageType.Info);
            }
        }
    }
}
