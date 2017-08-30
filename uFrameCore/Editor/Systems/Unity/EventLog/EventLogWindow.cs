using System;
using System.Collections.Generic;
using uFrame.Kernel;
using UnityEditor;
using UnityEngine;
using UniRx;

namespace uFrame.Editor.Unity.EventLog {
    public class EventLogWindow : EditorWindow {
        [MenuItem("Window/uFrame/Event Log")]
        private static void ShowWindow() {
            GetWindow<EventLogWindow>();
        }

        private EventDataList _events = new EventDataList();
        private int _selectedEventIndex = -1;
        private Vector2 _scrollbarPosition;
        private IDisposable _debugEventWrapperListener;
        private GUIStyle _eventButtonStyle;

        private const float kSplitRatio = 75f / 100f;
        private const float kSplitterHeight = 1f;
        private const float kLogItemHeight = 20f;
        private const float kPreviewPropertyHeight = 20f;

        private void OnEnable() {
            titleContent = new GUIContent("Event Log");
            
            if (EditorApplication.isPlaying) {
                RegisterDebugEventListener();
            } else {
                EditorApplication.playmodeStateChanged += PlaymodeStateChanged;
            }
        }
        
        private void OnDisable() {
            if (_debugEventWrapperListener != null) {
                uFrameKernel.EventAggregator.DebugEnabled = false;
                _debugEventWrapperListener.Dispose();
                _debugEventWrapperListener = null;
            }
        }
        
        private void RegisterDebugEventListener() {
            uFrameKernel.EventAggregator.DebugEnabled = true;
            _debugEventWrapperListener = 
                uFrameKernel.EventAggregator.GetEvent<DebugEventWrapperEvent>()
                    .Subscribe(evt => EventLogMediatorOnEventPublished(evt.Event));
        }

        private void PlaymodeStateChanged() {
            if (EditorApplication.isPlaying) {
                RegisterDebugEventListener();
                EditorApplication.playmodeStateChanged -= PlaymodeStateChanged;
            }
        }

        private void OnGUI() {
            if (_eventButtonStyle == null) {
                _eventButtonStyle = new GUIStyle(EditorStyles.miniButton);
                _eventButtonStyle.alignment = TextAnchor.MiddleLeft;
            }
            
            if (_selectedEventIndex >= _events.Count) {
                _selectedEventIndex = -1;
            }

            Rect currentRect = new Rect(0f, 0f, Screen.width, Mathf.RoundToInt(Screen.height * kSplitRatio));
            Rect viewRect = currentRect;
            viewRect.width -= 25f;
            viewRect.height = _events.Count * kLogItemHeight;
            if (Event.current.type != EventType.Layout) {
                _scrollbarPosition = GUI.BeginScrollView(currentRect, _scrollbarPosition, viewRect);
                {
                    Rect itemRect = currentRect;
                    itemRect.height = kLogItemHeight;
                    
                    for (int i = 0; i < _events.Count; i++) {
                        if (itemRect.yMax < _scrollbarPosition.y) {
                            itemRect.y += kLogItemHeight;
                            continue;
                        }
                        
                        if (itemRect.yMin > currentRect.y + currentRect.height + _scrollbarPosition.y) {
                            itemRect.y += kLogItemHeight;
                            break;
                        }
                        
                        EventData eventData = _events[i];
                        if (GUI.Toggle(itemRect, i == _selectedEventIndex, eventData.EventTypeName, _eventButtonStyle) && i != _selectedEventIndex) {
                            _selectedEventIndex = i;
                        }

                        itemRect.y += kLogItemHeight;
                    }
                }
                GUI.EndScrollView();
            }

            currentRect.yMin = currentRect.yMax;
            currentRect.height = kSplitterHeight;
            //GUI.Label(currentRect, "", EditorStyles.helpBox);
            GUI.color = new Color(0,0,0,0.5f);
            GUI.DrawTexture(currentRect, EditorGUIUtility.whiteTexture, ScaleMode.StretchToFill);
            GUI.color = Color.white;
            
            if (_selectedEventIndex != -1) {
                currentRect.yMin = currentRect.yMax;
                currentRect.height = Screen.height - currentRect.yMin;
                DrawPreview(currentRect);
            }

            Rect clearButtonRect = new Rect(Screen.width - 62f, Screen.height - 44f, 60f, 20f);
            if (GUI.Button(clearButtonRect, "Clear")) {
                _events.Clear();
                _selectedEventIndex = -1;
            }
        }

        private void DrawPreview(Rect rect) {
            GUIStyle labelStyle = new GUIStyle(GUI.skin.label);
            labelStyle.fontStyle = FontStyle.Normal;
            labelStyle.wordWrap = true;
            GUIStyle boldLabelStyle = new GUIStyle(GUI.skin.label);
            boldLabelStyle.fontStyle = FontStyle.Bold;

            GUI.BeginGroup(rect, GUI.skin.label);
            {
                GUILayout.BeginVertical();
                {
                    GUILayout.Space(3);
                    
                    EventData eventData = _events[_selectedEventIndex];
                    for (int i = 0; i < eventData.EventProperties.Length; i++) {
                        EventData.EventProperty eventProperty = eventData.EventProperties[i];
                        GUILayout.BeginHorizontal();
                        {
                            GUILayout.Label(eventProperty.Name + ":", boldLabelStyle, GUILayout.Height(kPreviewPropertyHeight));
                            if (eventProperty.UnityObject != null) {
                                if (GUILayout.Button(eventProperty.StringValue, GUILayout.Height(kPreviewPropertyHeight))) {
                                    EditorGUIUtility.PingObject(eventProperty.UnityObject);
                                }
                            } else {
                                GUILayout.Label(eventProperty.StringValue, labelStyle, GUILayout.Height(kPreviewPropertyHeight));
                            }
                            GUILayout.FlexibleSpace();
                        }
                        GUILayout.EndHorizontal();
                    }
                }
                GUILayout.EndVertical();
            }
            GUI.EndGroup();
        }

        private void EventLogMediatorOnEventPublished(object evt) {
            _events.Add(EventData.Build(evt));
            Repaint();
        }
        
        [Serializable]
        private class EventDataList : List<EventData> {
        }

        [Serializable]
        private class VerticalSplitter {
            public void OnGUI() {
                
            }
        }
    }
}