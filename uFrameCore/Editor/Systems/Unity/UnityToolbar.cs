using System;
using System.Linq;
using uFrame.Editor.Core;
using uFrame.Editor.GraphUI;
using uFrame.Editor.Platform;
using UnityEditor;
using UnityEngine;

namespace uFrame.Editor.Unity
{
    public class UnityToolbar : ToolbarUI
    {
        public override void Go()
        {
            base.Go();
               
            GUILayout.BeginHorizontal(EditorStyles.toolbar);
            foreach (var editorCommand in LeftCommands.OrderBy(p => p.Order))
            {
                DoCommand(editorCommand);
            }


            GUILayout.FlexibleSpace();

            foreach (var editorCommand in RightCommands.OrderBy(p => p.Order))
            {
                DoCommand(editorCommand);
            }
            
            GUILayout.EndHorizontal();
        }

        public override void GoBottom()
        {
            base.GoBottom();
            GUILayout.BeginHorizontal(EditorStyles.toolbar);
            //var scale = GUILayout.HorizontalSlider(ElementDesignerStyles.Scale, 0.8f, 1f, GUILayout.Width(200f));
            //if (scale != ElementDesignerStyles.Scale)
            //{
            //    ElementDesignerStyles.Scale = scale;
            //    InvertGraphEditor.ExecuteCommand(new ScaleCommand() { Scale = scale });

            //}
            foreach (var editorCommand in BottomLeftCommands.OrderBy(p => p.Order))
            {
                
                DoCommand(editorCommand);
            }
            GUILayout.FlexibleSpace();
            foreach (var editorCommand in BottomRightCommands.OrderBy(p => p.Order))
            {
                DoCommand(editorCommand);
            }
            GUILayout.EndHorizontal();
        }
        public void DoCommand(ToolbarItem command)
        {

            var style = EditorStyles.toolbarButton;
            if (command.IsDropdown)
            {
                style = EditorStyles.toolbarDropDown;
            }
            if (command.Checked)
            {
                style = new GUIStyle(EditorStyles.toolbarButton);
                style.normal.background = style.active.background;
            }
            
            var guiContent = new GUIContent(command.Title);
            if (GUILayout.Button(guiContent, style))
            {
                if (command.IsDelayCall) // Fix the invalide status, stack error
                {
                    EditorApplication.delayCall += () => { InvertApplication.Execute(command.Command); };
                }
                else
                {
                    InvertApplication.Execute(command.Command);
                }
            }
            InvertGraphEditor.PlatformDrawer.SetTooltipForRect(GUILayoutUtility.GetLastRect(),command.Description);
            
            GUI.enabled = true;
        }
    }
}