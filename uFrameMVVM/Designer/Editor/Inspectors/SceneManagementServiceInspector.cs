using System;
using uFrame.Editor.Unity;
using uFrame.Kernel;
using UnityEditor;
using UnityEngine;

namespace uFrame.MVVM.Editor {
    [CustomEditor(typeof(SceneManagementService), true)]
    public class SceneManagementServiceInspector : ManagerInspector<SceneManagementService> {
        private SceneManagementService _service;

        public SceneManagementService Service {
            get {
                return _service ?? (_service = uFrameKernel.Container.Resolve<SceneManagementService>());
            }
        }

        public void Warning(string message) {
            EditorGUILayout.HelpBox(message, MessageType.Warning);
        }

        public void OnDisable() {
            _service = null;
        }

        public override void OnInspectorGUI() {
            GUIHelpers.IsInspector = true;
            //
            DrawTitleBar("Scene Management Service");
            base.OnInspectorGUI();
            serializedObject.Update();

            if (Application.isPlaying) {
                if (GUIHelpers.DoToolbarEx("Loaded Scenes")) {
                    if (Service != null) {
                        foreach (IScene instance in Service.LoadedScenes) {
                            if (GUIHelpers.DoTriggerButton(new UFStyle {
                                BackgroundStyle = ElementDesignerStyles.EventButtonStyleSmall,
                                Label = string.Format("{0}", instance.GetType().Name)
                            })) {
                                Selection.activeGameObject = (instance as MonoBehaviour).gameObject;
                            }
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
