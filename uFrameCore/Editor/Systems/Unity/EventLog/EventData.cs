using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Object = UnityEngine.Object;

namespace uFrame.Editor.Unity.EventLog {
    [Serializable]
    public class EventData {
        private string _eventTypeName;
        private EventProperty[] _eventProperties;

        public string EventTypeName {
            get {
                return _eventTypeName;
            }
            private set {
                _eventTypeName = value;
            }
        }

        public EventProperty[] EventProperties {
            get {
                return _eventProperties;
            }
            private set {
                _eventProperties = value;
            }
        }

        public static EventData Build(object evt) {
            Type eventType = evt.GetType();

            EventData eventData = new EventData();
            TypeReflectionInfoProvider.TypeReflectionInfo reflectionInfo =
                TypeReflectionInfoProvider.GetReflectionInfo(eventType);

            List<EventProperty> properties = new List<EventProperty>();
            foreach (FieldInfo fieldInfo in reflectionInfo.Fields) {
                properties.Add(EventProperty.Build(fieldInfo.Name, fieldInfo.GetValue(evt)));
            }

            foreach (PropertyInfo propertyInfo in reflectionInfo.Properties) {
                properties.Add(EventProperty.Build(propertyInfo.Name, propertyInfo.GetValue(evt, null)));
            }

            eventData.EventProperties = properties.ToArray();
            StringBuilder nameBuilder = new StringBuilder();
            nameBuilder.Append(eventType.FullName);
            if (properties.Count > 0) {
                nameBuilder.Append(" (");
                for (int i = 0; i < properties.Count; i++) {
                    EventProperty eventProperty = properties[i];
                    nameBuilder.AppendFormat("{0}: {1}", eventProperty.Name, eventProperty.StringValue);
                    if (i != properties.Count - 1) {
                        nameBuilder.Append(", ");
                    }
                }
                nameBuilder.Append(" )");
            }
            eventData.EventTypeName = nameBuilder.ToString();

            return eventData;
        }
        
        [Serializable]
        public class EventProperty {
            private string _name;
            private object _object;
            private string _stringValue;
            private Object _unityObject;

            public string Name {
                get {
                    return _name;
                }
                private set {
                    _name = value;
                }
            }

            public string StringValue {
                get {
                    return _stringValue;
                }
                private set {
                    _stringValue = value;
                }
            }

            public Object UnityObject {
                get {
                    return _unityObject;
                }
                private set {
                    _unityObject = value;
                }
            }

            public object Object {
                get {
                    return _object;
                }
                set {
                    _object = value;
                }
            }

            public static EventProperty Build(string name, object value) {
                EventProperty eventProperty = new EventProperty();
                eventProperty.Name = name;

                if (value == null) {
                    eventProperty.StringValue = "null";
                } else {
                    eventProperty.Object = value;
                    Object unityObject = value as Object;
                    if (unityObject != null) {
                        eventProperty.UnityObject = unityObject;
                    }

                    eventProperty.StringValue = value.ToString();
                }

                return eventProperty;
            }
        }
        
        private static class TypeReflectionInfoProvider {
            private static readonly Dictionary<Type, TypeReflectionInfo> _typeReflectionData = new Dictionary<Type, TypeReflectionInfo>();

            public static TypeReflectionInfo GetReflectionInfo(Type type) {
                TypeReflectionInfo info;
                if (!_typeReflectionData.TryGetValue(type, out info)) {
                    PropertyInfo[] properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
                    FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public);

                    info = new TypeReflectionInfo(properties, fields);
                    _typeReflectionData.Add(type, info);
                }

                return info;
            }

            public class TypeReflectionInfo {
                public PropertyInfo[] Properties { get; private set; }
                public FieldInfo[] Fields { get; private set; }

                public TypeReflectionInfo(PropertyInfo[] properties, FieldInfo[] fields) {
                    Properties = properties;
                    Fields = fields;
                }
            }
        }
    }
}