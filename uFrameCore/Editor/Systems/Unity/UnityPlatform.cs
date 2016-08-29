using System;
using uFrame.Editor.Core;
using uFrame.Editor.Platform;
using UnityEditor;
using UnityEngine;

namespace uFrame.Editor.Unity
{
    public class UnityPlatform : DiagramPlugin, IPlatformOperations, IDebugLogger
    {

        //public void ShowFileDialog(string title)
        //{
        //    EditorUtility.OpenFilePanel(title,directory,)
        //}

        public void OpenScriptFile(string filePath)
        {
            var scriptAsset = AssetDatabase.LoadAssetAtPath(filePath, typeof(TextAsset));
            AssetDatabase.OpenAsset(scriptAsset);
        }

        public void OpenLink(string link)
        {
            Application.OpenURL(link);
        }

        public string GetAssetPath(object graphData)
        {
            return AssetDatabase.GetAssetPath(graphData as UnityEngine.Object);
        }
        public bool MessageBox(string title, string message, string ok)
        {
            return EditorUtility.DisplayDialog(title, message, ok);
        }
        public bool MessageBox(string title, string message, string ok, string cancel)
        {
            return EditorUtility.DisplayDialog(title, message, ok, cancel);
        }

        public void ComplexMessageBox(string title, string message, string optionA, Action actionA, 
                                                                    string optionB, Action actionB, 
                                                                    string optionC, Action actionC)
        {
            int option = EditorUtility.DisplayDialogComplex(title, message, optionA, optionB, optionC);

            switch (option)
            {
                case 0:
                    if (actionA != null) actionA();
                    break;
                case 1:
                    if (actionB != null) actionB();
                    break;
                case 2:
                    if (actionC != null) actionC();
                    break;
            }
        }

        public void SaveAssets()
        {
            AssetDatabase.SaveAssets();
        }

        public void RefreshAssets()
        {
            AssetDatabase.Refresh();
            //AssetDatabase.Refresh();
        }

        public void Progress(float progress, string message)
        {
            try
            {
                InvertApplication.SignalEvent<ITaskProgressHandler>(_=>_.Progress(progress, message));
                //if (progress > 100f)
                //{
                //    EditorUtility.ClearProgressBar();
                //    return;
                //}
                //EditorUtility.DisplayProgressBar("Generating", message, progress/1f);
            }
            catch (Exception ex)
            {
                
            }
        }

        public void Log(string message)
        {
            Debug.Log(message); 
        }

        public void LogException(Exception ex)
        {
            Debug.LogException(ex);
            if (ex.InnerException != null)
            {
                Debug.LogException(ex.InnerException);
            }
        }
    }
}
