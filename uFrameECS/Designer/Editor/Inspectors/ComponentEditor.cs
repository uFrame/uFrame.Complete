using System;
using System.Linq;
using uFrame.ECS.Components;
using uFrame.Editor.Core;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace uFrame.ECS.Editor
{
    [UnityEditor.CustomEditor(typeof(EcsComponent), true)]
    public class ComponentEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            //base.OnInspectorGUI();

            var properties = target.GetType().GetProperties();
            serializedObject.Update();
            SerializedProperty iterator = serializedObject.GetIterator();
            for (bool enterChildren = true; iterator.NextVisible(enterChildren); enterChildren = false)
            {
                EditorGUI.BeginChangeCheck();

                if (iterator.isArray && iterator.propertyType != SerializedPropertyType.String)
                {
                    var list = new ReorderableList(iterator.serializedObject, iterator, true, true, true, true);
                    list.drawHeaderCallback += rect => GUI.Label(rect, iterator.displayName);
                    list.drawElementCallback += (rect, index, active, focused) =>
                    {
                        rect.height = 16;
                        rect.y += 2;
                        EditorGUI.PropertyField(rect,
                            list.serializedProperty.GetArrayElementAtIndex(index),
                            GUIContent.none);
                    };

                    list.DoLayoutList();
                    // list.DoList(EditorGUILayout.);
                }
                else
                {
                    EditorGUILayout.PropertyField(iterator, true, new GUILayoutOption[0]);
                }
                if (EditorGUI.EndChangeCheck())
                {
                    var propertyName = iterator.name.ToLower().Substring(1);

                    var propertyInfo = properties.FirstOrDefault(p => p.Name.ToLower() == propertyName);
                    if (propertyInfo != null)
                    {
                        if (propertyInfo.PropertyType == typeof(int))
                        {
                            propertyInfo.SetValue(target, iterator.intValue, null);
                        }
                        if (propertyInfo.PropertyType == typeof(AnimationCurve))
                        {
                            propertyInfo.SetValue(target, iterator.animationCurveValue, null);
                        }
                        if (propertyInfo.PropertyType == typeof(Color))
                        {
                            propertyInfo.SetValue(target, iterator.colorValue, null);
                        }
                        if (propertyInfo.PropertyType == typeof(Quaternion))
                        {
                            propertyInfo.SetValue(target, iterator.quaternionValue, null);
                        }
                        if (propertyInfo.PropertyType == typeof(float))
                        {
                            propertyInfo.SetValue(target, iterator.floatValue, null);
                        }
                        if (propertyInfo.PropertyType == typeof(bool))
                        {
                            propertyInfo.SetValue(target, iterator.boolValue, null);
                        }
                        if (propertyInfo.PropertyType == typeof(Bounds))
                        {
                            propertyInfo.SetValue(target, iterator.boundsValue, null);
                        }
                        if (propertyInfo.PropertyType == typeof(string))
                        {
                            propertyInfo.SetValue(target, iterator.stringValue, null);
                        }
                        if (propertyInfo.PropertyType == typeof(Vector2))
                        {
                            propertyInfo.SetValue(target, iterator.vector2Value, null);
                        }
                        if (propertyInfo.PropertyType == typeof(Vector3))
                        {
                            propertyInfo.SetValue(target, iterator.vector3Value, null);
                        }
                        if (propertyInfo.PropertyType == typeof(Vector4))
                        {
                            propertyInfo.SetValue(target, iterator.vector4Value, null);
                        }

                        if (typeof(Enum).IsAssignableFrom(propertyInfo.PropertyType))
                        {
                            propertyInfo.SetValue(target, Enum.GetValues(propertyInfo.PropertyType).GetValue(iterator.enumValueIndex), null);
                        }
                    }
                }
            }
            serializedObject.ApplyModifiedProperties();
            if (InvertApplication.Container != null)
            {
                InvertApplication.SignalEvent<IDrawUnityInspector>(_ => _.DrawInspector(target));
            }

        }
    }
}
