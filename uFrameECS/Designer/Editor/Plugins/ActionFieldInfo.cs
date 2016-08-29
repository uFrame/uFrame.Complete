using System;
using System.Collections.Generic;
using System.Linq;
using uFrame.Attributes;
using uFrame.Editor.Graphs.Data.Types;

namespace uFrame.ECS.Editor
{
    public interface IActionFieldInfo : IMemberInfo
    {
        string Name { get; }
        string Description { get; }
        bool IsGenericArgument { get; }
        bool IsReturn { get; }
        bool IsByRef { get; }
        FieldDisplayTypeAttribute DisplayType { get; }
        bool IsBranch { get; }
        bool IsOptional { get; }
        IEnumerable<IMemberInfo> DelegateMembers { get; }
        bool IsDelegateMember { get; set; }
    }

    public class ActionFieldInfo : IActionFieldInfo
    {
        public string Name
        {
            get
            {
                if (DisplayType == null) return _name;
                return DisplayType.DisplayName ?? _name;
            }
            set { _name = value; }
        }

        public string Description
        {
            get
            {
                if (_description == null)
                {
                    if (MetaAttributes == null) return null;
                    var descriptionContainer =
                        MetaAttributes.OfType<Description>().FirstOrDefault();
                    if (descriptionContainer != null)
                    {
                        _description = descriptionContainer.Text;
                    }
                    else
                    {
                        _description = "";
                    }
                }; return _description;
            }
            set { _description = value; }
        }

        private FieldDisplayTypeAttribute _displayType;
        private string _name;
        private string _description;

        public ActionAttribute[] MetaAttributes { get; set; }

        public FieldDisplayTypeAttribute DisplayType
        {
            get { return _displayType ?? (_displayType = MetaAttributes == null ? null : MetaAttributes.OfType<FieldDisplayTypeAttribute>().FirstOrDefault()); }
            set { _displayType = value; }
        }

        public bool IsBranch { get; set; }

        public bool IsOptional { get; set; }

        public bool IsGenericArgument { get; set; }
        public bool IsReturn { get; set; }
        public bool IsByRef { get; set; }


        public string MemberName { get; set; }
        public ITypeInfo MemberType { get; set; }
        public IEnumerable<Attribute> GetAttributes()
        {
            if (MetaAttributes == null)
                yield break;

            foreach (var item in MetaAttributes)
                yield return item;
        }


        public IEnumerable<IMemberInfo> DelegateMembers
        {
            get
            {
                var member = MemberType.GetMembers().OfType<IMethodMemberInfo>().FirstOrDefault(p => p.MemberName == "Invoke");
                if (member == null) yield break;
                var parameters = member.GetParameters();
                foreach (var item in parameters)
                {
                    yield return item;
                    
                }
            }
        }

        public bool IsDelegateMember { get; set; }
    }

    
}