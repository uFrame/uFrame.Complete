using System.Collections.Generic;
using System.Linq;
using uFrame.Editor.Configurations;
using uFrame.Editor.Graphs.Data;

namespace uFrame.Editor.Compiling.CommonNodes
{
    public class EnumNode : GenericNode, IClassTypeNode
    {
        public override bool AllowOutputs
        {
            get { return false; }
        }

        public string ClassName
        {
            get { return Name; }
        }

        [Section("Enum Items", SectionVisibility.Always)]
        public IEnumerable<EnumChildItem> Items
        {
            get
            {
                return PersistedItems.OfType<EnumChildItem>();
            }
        }

        public override bool IsEnum
        {
            get { return true; }
        }
    }
}