using uFrame.ECS.Editor;
using uFrame.Editor.Attributes;
using uFrame.Editor.Database.Data;
using uFrame.Editor.Graphs.Data;
using uFrame.Editor.Nodes;
using uFrame.Json;

namespace uFrame.ECS.Editor
{
    using System.Collections.Generic;
    

    public class EventIds : IntegerIdProvider { }

    public interface IEventId : IDiagramNode
    {
        int EventId { get; set; }
    }
    public class EventNode : EventNodeBase, IEventMetaInfo, IDemoVersionLimit, IClassNode, IEventId
    {
        private int _eventId;

        [JsonProperty, InspectorProperty]
        public int EventId
        {
            get
            {
                if (_eventId == 0)
                {
                    _eventId = Repository.GetSingleLazy<EventIds>().NextId;
                }
                return _eventId;
            }
            set { this.Changed("EventId", ref _eventId, value); }
        }

        public override void Validate(List<ErrorInfo> errors)
        {
            base.Validate(errors);

        }

        public string Category
        {
            get
            {
                return Namespace;
            }
        }

        public bool Dispatcher
        {
            get { return false; }
            set { this["Dispatcher"] = value; }
        }

        public override string ClassName
        {
            get {
                if (Dispatcher)
                {
                    return string.Format("{0}Dispatcher",Name);
                }
                return Name;
            }
        }



        //[InspectorProperty]
        //public bool NeedsMappings
        //{
        //    get { return this["NeedsMappings"]; }
        //    set { this["NeedsMappings"] = value; }
        //}

        [InspectorProperty]
        public bool SystemEvent
        {
            get { return false; }
            set {  }
        }

        public string SystemEventMethod
        {
            get { return null; }
        }


    }
    
    public partial interface IEventConnectable : IDiagramNodeItem, IConnectable {
    }
}
