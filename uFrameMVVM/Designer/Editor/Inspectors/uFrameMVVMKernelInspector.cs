using System;
using System.Collections.Generic;
using System.Linq;
using uFrame.Editor.Unity;
using uFrame.Kernel;
using UnityEditor;
using UnityEngine;

namespace uFrame.MVVM.Editor {
    [CustomEditor(typeof(uFrameKernel), true)]
    public class UFrameMvvmKernelInspector : ManagerInspector<uFrameKernel> {
        public void Warning(string message) {
            EditorGUILayout.HelpBox(message, MessageType.Warning);
        }

        public override void OnInspectorGUI() {
            GUIHelpers.IsInspector = true;
            //base.OnInspectorGUI();
            DrawTitleBar("UFrame MVVM Kernel");
            serializedObject.Update();

            if (!EditorBuildSettings.scenes.Any(s => {
                return s.path.EndsWith("KernelScene.unity");
            })) {
                Warning("Please add this scene to the build settings!");
            }

            if (Application.isPlaying) {
                if (!uFrameKernel.IsKernelLoaded) {
                    Warning("Kernel is not loaded!");
                }
                if (uFrameKernel.Instance == null) return;

                if (GUIHelpers.DoToolbarEx("Services")) {
                    foreach (ISystemService instance in uFrameKernel.Instance.Services) {
                        if (GUIHelpers.DoTriggerButton(new UFStyle {
                            BackgroundStyle = ElementDesignerStyles.EventButtonStyleSmall,
                            Label = string.Format("{0}", instance.GetType().Name)
                        })) {
                            Selection.activeGameObject = (instance as MonoBehaviour).gameObject;
                        }
                    }
                }

                if (GUIHelpers.DoToolbarEx("Systems")) {
                    foreach (ISystemLoader instance in uFrameKernel.Instance.SystemLoaders) {
                        if (GUIHelpers.DoTriggerButton(new UFStyle {
                            BackgroundStyle = ElementDesignerStyles.EventButtonStyleSmall,
                            Label = string.Format("{0}", instance.GetType().Name.Replace("Loader", ""))
                        })) {
                            Selection.activeGameObject = (instance as MonoBehaviour).gameObject;
                        }
                    }
                }

                if (GUIHelpers.DoToolbarEx("Scene Loaders")) {
                    if (GUIHelpers.DoTriggerButton(new UFStyle {
                        BackgroundStyle = ElementDesignerStyles.EventButtonStyleSmall,
                        Label = string.Format("{0}", "DefaultSceneLoader")
                    })) {
                    }

                    //foreach (var instance in uFrameMVVMKernel.Instance.SceneLoaders)
                    //{
                    //    if (GUIHelpers.DoTriggerButton(new UFStyle()
                    //    {
                    //        BackgroundStyle = ElementDesignerStyles.EventButtonStyleSmall,
                    //        Label = string.Format("{0}", instance.GetType().Name)
                    //    }))
                    //    {
                    //        Selection.activeGameObject = (instance as MonoBehaviour).gameObject;
                    //    }
                    //}
                }

                if (GUIHelpers.DoToolbarEx(String.Format("Dependency Container - Instances ({0})", uFrameKernel.Container.Instances.Count), defOn: false, prefsKey: "Dependency Container - Instances")) {
                    foreach (KeyValuePair<Tuple<Type, string>, object> instance in uFrameKernel.Container.Instances) {
                        if (GUIHelpers.DoTriggerButton(new UFStyle {
                            Label =
                                string.Format("'{0}': {1}->{2}", instance.Key.Item1, instance.Key.Item2,
                                    instance.Value.GetType().Name),
                            BackgroundStyle = ElementDesignerStyles.EventButtonStyleSmall
                        })) {
                            Debug.Log(instance.Value);
                        }
                    }
                }

                if (GUIHelpers.DoToolbarEx(String.Format("Dependency Container - Mappings ({0})", uFrameKernel.Container.Mappings.Count), defOn: false, prefsKey: "Dependency Container - Mappings")) {
                    foreach (KeyValuePair<Tuple<Type, string>, Type> instance in uFrameKernel.Container.Mappings) {
                        if (GUIHelpers.DoTriggerButton(new UFStyle {
                            BackgroundStyle = ElementDesignerStyles.EventButtonStyleSmall,
                            Label = string.Format("{0}: {1}->{2}", instance.Key.Item2, instance.Key.Item1.Name, instance.Value.Name)
                        })) {
                        }
                    }
                }
            }

            if (serializedObject.ApplyModifiedProperties()) {
                //var t = Target as GameManager;
                //t.ApplyRenderSettings();
            }
            GUIHelpers.IsInspector = false;
        }

        protected override bool ExistsInScene(Type itemType) {
            return FindObjectOfType(itemType) != null;
        }

        protected override string GetTypeNameFromName(string name) {
            return name + "Game";
        }
    }
}
