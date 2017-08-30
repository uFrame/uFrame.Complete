using uFrame.Editor.Core;
using uFrame.Editor.GraphUI;
using UnityEditor;
using UnityEngine;

namespace uFrame.Editor.Unity
{
    public class ElementsDesigner : EditorWindow
    {
        public static ElementsDesigner Instance { get; set; }

        public bool IsFocused { get; set; }
        public bool IsVisible { get; set; }

        [MenuItem("Window/uFrame/Graph Window #&u", false, 1)]
        public static void Init()
        {
            // Get existing open window or if none, make a new one:
            var window = (ElementsDesigner)GetWindow(typeof(ElementsDesigner));
            //window.title = "uFrame";
            window.titleContent.text = "uFrame";

            window.Show();
            window.Repaint();
            window.wantsMouseMove = true;
            Instance = window;
        }
        public void InfoBox(string message, MessageType type = MessageType.Info)
        {
            EditorGUI.HelpBox(new Rect(15, 30, 300, 30), message, type);
        }


        public void OnEnable()
        {

        }

        public void OnGUI() {
            EventType currentType = Event.current.type;
            if (InvertGraphEditor.Container != null)
            {
                InvertApplication.SignalEvent<IDrawDesignerWindow>(_=>_.DrawDesigner(position.width, position.height));
            }

            if (currentType == EventType.MouseMove || currentType == EventType.MouseDrag) {
                Repaint();
            }
        }

        public void OnFocus()
        {
            IsFocused = true;
        }

        public void OnLostFocus()
        {
            InvertApplication.SignalEvent<IDesignerWindowLostFocus>(_=>_.DesignerWindowLostFocus());

            IsFocused = false;
        }

        private void OnBecameVisible()
        {
            IsVisible = true;
        }

        private void OnBecameInvisible() {
            IsVisible = false;
        }

        public void OnInspectorUpdate()
        {
            InvertApplication.SignalEvent<IUpdate>(_ => _.Update());
            if (IsVisible)
            {
                Repaint();
            }
            //if (EditorApplication.isPlaying)
            //{
            //    Instance = this;
            //    InvertApplication.SignalEvent<IUpdate>(_ => _.Update());
            //    Repaint();
            //}
        }
        public void Update()
        {
            //if (!EditorApplication.isPlaying || EditorApplication.isPaused)
            //{
                //Instance = this;
            InvertApplication.SignalEvent<IUpdate>(_ => _.Update());
            //if(mouseOverWindow) {
            //    Repaint();
            //}
            //}
        }


    }
}