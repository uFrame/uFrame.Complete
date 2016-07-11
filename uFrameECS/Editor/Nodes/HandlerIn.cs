using System.Collections.Generic;
using System.Linq;
using uFrame.Attributes;
using uFrame.Editor.Database.Data;
using uFrame.Editor.Graphs.Data;
using uFrame.Editor.Graphs.Data.Types;

namespace uFrame.ECS.Editor
{
    public class EntityGroupIn : SelectionFor<IMappingsConnectable, HandlerInValue>, IFilterInput
    {
        public IMappingsConnectable FilterNode
        {
            get { return this.Item; }
        }

        public virtual string MappingId
        {
            get { return "EntityId"; }
        }

        public override string Name
        {
            get { return "Group"; }
            set { base.Name = value; }
        }

        public override string Title
        {
            get { return "Group"; }
        }

        public virtual string HandlerPropertyName
        {
            get { return Name; }
        }

        public override IEnumerable<IValueItem> GetAllowed()
        {
            return Repository.AllOf<IMappingsConnectable>().OfType<IValueItem>();
        }
        public override bool AllowInputs
        {
            get { return false; }
        }

    }

    public class HandlerIn : EntityGroupIn, IFilterInput
    {
        private uFrameEventMapping _uFrameEventMapping;

        public override string MappingId
        {
            get { return EventFieldInfo.MemberName; }
        }

        public IMemberInfo EventFieldInfo { get; set; }
        public override string Title
        {
            get
            {
                if (UFrameEventMapping != null)
                {
                    return UFrameEventMapping.Title;
                }
                return EventFieldInfo.MemberName;
            }
        }
        public override string Name
        {
            get { return Title; }
            set { base.Name = value; }
        }

        public override string HandlerPropertyName
        {
            get { return Name; }
        }

        public uFrameEventMapping UFrameEventMapping
        {
            get { return _uFrameEventMapping ?? (_uFrameEventMapping = EventFieldInfo.GetAttribute<uFrameEventMapping>()); }
            set { _uFrameEventMapping = value; }
        }
    }

    public class HandlerInValue : InputSelectionValue
    {
        
    }
}