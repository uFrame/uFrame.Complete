using System;
using uFrame.Editor.Unity;
using uFrame.Kernel;
using uFrame.MVVM.Services;
using UnityEditor;
using UnityEngine;

namespace uFrame.MVVM.Editor {
    [CustomEditor(typeof(ViewService), true)]
    public class ViewServiceInspector : ManagerInspector<ViewService> {
        private ViewService _viewService;

        public void Warning(string message) {

            EditorGUILayout.HelpBox(message, MessageType.Warning);
        }

        public ViewService ViewService {
            get {
                return _viewService ?? (_viewService = uFrameKernel.Container.Resolve<ViewService>());
            }
        }

        public void OnDisable() {
            _viewService = null;
        }

        public override void OnInspectorGUI() {
            GUIHelpers.IsInspector = true;
            //base.OnInspectorGUI();
            DrawTitleBar("View Service");
            serializedObject.Update();

            if (Application.isPlaying && ViewService != null) {
                if (GUIHelpers.DoToolbarEx(String.Format("Views ({0})", ViewService.Views.Count), defOn: false, prefsKey: "ViewServiceInspectorViews")) {
                    foreach (var instance in ViewService.Views) {
                        EditorGUILayout.BeginHorizontal();
                        {
                            EditorGUILayout.PrefixLabel(instance.GetType().Name);
                            EditorGUILayout.ObjectField(instance as MonoBehaviour, typeof(MonoBehaviour), true);
                        }
                        EditorGUILayout.EndHorizontal();
                    }
                }
            } else {
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