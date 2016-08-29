using System;
using System.Collections.Generic;
using System.Linq;
using uFrame.Attributes;
using uFrame.ECS.Editor;
using uFrame.Editor.Core;
using uFrame.Editor.Graphs.Data.Types;

namespace uFrame.ECS.Editor
{
    public interface IEventMetaInfo : IItem, ITypeInfo
    {
        string Category { get; }
        bool Dispatcher { get; }
        bool SystemEvent { get; }
        string SystemEventMethod { get; }
    }

    public class EventMetaInfo : SystemTypeInfo, IEventMetaInfo
    {
        private List<EventFieldInfo> _members;
        private uFrameCategory _categoryAttribute;

        public uFrameEvent Attribute { get; set; }

        public uFrameCategory CategoryAttribute
        {
            get { return _categoryAttribute ?? (_categoryAttribute = SystemType.GetCustomAttributes(typeof(uFrameCategory), true).OfType<uFrameCategory>().FirstOrDefault()); }
            set { _categoryAttribute = value; }
        }

        public string Category
        {
            get
            {
                if (CategoryAttribute != null)
                {
                    return CategoryAttribute.Title.FirstOrDefault() ?? "Listen For";
                }
                return "Listen For";
            }
        }
        public bool Dispatcher
        {
            get { return Attribute is UFrameEventDispatcher; }
        }

        public bool SystemEvent
        {
            get { return Attribute is SystemUFrameEvent; }
        }

        //public List<EventFieldInfo> Members
        //{
        //    get { return _members ?? (_members = new List<EventFieldInfo>()); }
        //    set { _members = value; }
        //}

        public string SystemEventMethod
        {
            get { return (Attribute as SystemUFrameEvent).SystemMethodName; }
        }

        public IHandlerCodeWriter CodeWriter { get; set; }

        public override string Title
        {
            get
            {
                if (SystemEvent) return (Attribute as SystemUFrameEvent).Title;
                if (Attribute != null)
                {
                    return Attribute.Title;
                }
                return SystemType.Name;
            }
        }

        public EventMetaInfo(Type systemType) : base(systemType)
        {
        }

    }
}