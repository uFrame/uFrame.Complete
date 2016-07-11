using uFrame.ECS.Systems;
using uFrame.Editor.Core;
using UnityEditor;

[UnityEditor.CustomEditor(typeof(EcsSystem),true)]
public class SystemEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        InvertApplication.SignalEvent<IDrawUnityInspector>(_ => _.DrawInspector(target));
    }
}