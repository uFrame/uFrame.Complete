using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using uFrame.Editor.Unity;
using uFrame.MVVM.Attributes;
using uFrame.MVVM.Views;
using UnityEditor;
using UnityEngine;

namespace uFrame.MVVM.Editor
{
    public class uFrameInspector : UnityEditor.Editor
    {
        protected Dictionary<string, FieldInfo> _toggleGroups;
        protected Dictionary<string, List<FieldInfo>> _groupFields;
        private FieldInfo[] _fieldInfos;

        public static void DrawTitleBar(string subTitle)
        {
            ElementDesignerStyles.DoTilebar(subTitle);
            EditorGUILayout.Space();
        }

        public string ReflectionPopup(string label, SerializedProperty prop, string[] properties)
        {
            var index = Array.IndexOf(properties, prop.stringValue);
            var newIndex = EditorGUILayout.Popup(label, index, properties);

            if (newIndex != index)
            {
                prop.stringValue = properties[newIndex];
            }
            return prop.stringValue;
        }

        public bool Toggle(string text, bool open, bool allowCollapse = true)
        {
            var result = GUIHelpers.DoToolbar(text, open);

            if (open || result)
            {
                EditorGUILayout.Space();
                if (result) return !open;
            }

            return open;
        }

        public void Section(string text)
        {
            var rect = GUIHelpers.GetRect(ElementDesignerStyles.SubHeaderStyle);
            GUI.Toggle(rect, true, text, ElementDesignerStyles.SubHeaderStyle);
            EditorGUILayout.Space();
            EditorGUI.indentLevel++;
        }

        public void EndSection()
        {
            EditorGUI.indentLevel--;
        }

        protected void GetFieldInformation(ViewBase t)
        {
            _fieldInfos = t.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
            _toggleGroups = new Dictionary<string, FieldInfo>();
            _groupFields = new Dictionary<string, List<FieldInfo>>();
            foreach (var fieldInfo in _fieldInfos)
            {
                var attribute = fieldInfo.GetCustomAttributes(typeof(UFToggleGroup), true).FirstOrDefault() as UFToggleGroup;
                var requireInstanceAttribute =
                    fieldInfo.GetCustomAttributes(typeof(UFRequireInstanceMethod), true).FirstOrDefault() as UFRequireInstanceMethod;

                if (requireInstanceAttribute != null)
                {
                    var method = t.GetType()
                        .GetMethod(requireInstanceAttribute.MethodName, BindingFlags.Public | BindingFlags.Instance);

                    if (method == null || (method.DeclaringType != null
                        && method.DeclaringType.Name.EndsWith("ViewBase"))) // TODO: Remove this hack and use attributes
                    {
                        var value = (bool)fieldInfo.GetValue(target);
                        if (value)
                        {
                            fieldInfo.SetValue(target, false);
                        }
                        continue;
                    }
                }

                if (attribute == null)
                {
                    var groupAttribute =
                        fieldInfo.GetCustomAttributes(typeof(UFGroup), true).FirstOrDefault() as UFGroup;
                    if (groupAttribute == null) continue;
                    if (!_groupFields.ContainsKey(groupAttribute.Name))
                    {
                        _groupFields.Add(groupAttribute.Name, new List<FieldInfo>());
                    }
                    _groupFields[groupAttribute.Name].Add(fieldInfo);
                }
                else
                {
                    if (!_toggleGroups.ContainsKey(attribute.Name))
                    {
                        _toggleGroups.Add(attribute.Name, fieldInfo);
                    }
                    if (!_groupFields.ContainsKey(attribute.Name))
                    {
                        _groupFields.Add(attribute.Name, new List<FieldInfo>());
                    }
                }
            }

        }
    }
}
