using uFrame.Editor.Attributes;
using uFrame.Editor.Graphs.Data;
using uFrame.Editor.Database.Data;
using uFrame.Json;

namespace uFrame.Architect.Editor.Data
{
    public class ShellInheritableNode : GenericInheritableNode, IShellNode
    {
        private string _baseIdentifier;

        [JsonProperty]
        public string BaseIdentifier
        {
            get { return _baseIdentifier; }
            set
            {
                this.Changed("BaseIdentifier", ref _baseIdentifier, value);
            }
        }

        [InspectorProperty(InspectorType.GraphItems)]
        public override GenericInheritableNode BaseNode
        {
            get
            {
                if (string.IsNullOrEmpty(BaseIdentifier) || Repository == null)
                    return null;
                return Repository.GetById<GenericInheritableNode>(BaseIdentifier);
            }
            set
            {
                if (value != null)
                    BaseIdentifier = value.Identifier;
                else
                {
                    BaseIdentifier = null;
                }
            }
        }

        public virtual string ClassName
        {
            get { return string.Format("{0}", Name); }
        }


    }
}