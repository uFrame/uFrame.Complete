using uFrame.Editor.Attributes;
using uFrame.Editor.Graphs.Data;

namespace uFrame.Architect.Editor.Data
{
    public class ShellNode : GenericNode, IShellNode
    {

        [InspectorProperty]
        public bool IsCustom
        {
            get { return this["Custom"]; }
            set { this["Custom"] = value; }
        }


        public virtual string ClassName
        {
            get { return string.Format("{0}", Name); }
        }
    }
}