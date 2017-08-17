using System;
using uFrame.Editor.Compiling.CodeGen;
using uFrame.Editor.Compiling.Events;
using uFrame.Editor.Database;
using uFrame.Editor.Database.Data;
using uFrame.Editor.GraphUI;
using uFrame.Editor.GraphUI.Events;
using uFrame.Editor.Platform;
using uFrame.IOC;
using UnityEditor;
using UnityEngine;

namespace uFrame.Editor.Unity
{
    public class CompilationProgress : DiagramPlugin, ITaskProgressEvent, ICompileEvents
    {
        public override bool Required
        {
            get { return true; }
        }

        public override void Initialize(UFrameContainer container)
        {
            Percentage = 0f;
        }

        public string Message { get; set; }
        public float Percentage { get; set; }

        public void Progress(float progress, string message, bool modal)
        {
            Message = message;
            Percentage = progress / 100f;
            Modal = modal;

            if (!String.IsNullOrEmpty(message)) {
                EditorUtility.DisplayProgressBar("Build", Message, Percentage);
            } else {
                EditorUtility.ClearProgressBar();
            }
        }

        public bool Modal { get; set; }


        public void PreCompile(IGraphConfiguration configuration, IDataRecord[] compilingRecords)
        {
            EditorApplication.LockReloadAssemblies();
        }

        public void PostCompile(IGraphConfiguration configuration, IDataRecord[] compilingRecords)
        {
            EditorApplication.UnlockReloadAssemblies();
            EditorUtility.ClearProgressBar();
        }

        public void FileGenerated(CodeFileGenerator generator)
        {

        }

        public void FileSkipped(CodeFileGenerator codeFileGenerator)
        {

        }
    }
}