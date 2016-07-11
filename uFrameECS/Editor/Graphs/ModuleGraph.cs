using uFrame.ECS.Editor;
using uFrame.Editor.Core;
using uFrame.Editor.Database.Data;
using uFrame.Json;

namespace uFrame.ECS.Editor
{
    using System.Collections.Generic;

    public class ModuleGraph : ModuleGraphBase, IVariableNameProvider {
        private int _variableCount;

        [JsonProperty]//,InspectorProperty]
        public int VariableCount
        {
            get { return _variableCount; }
            set { this.Changed("VariableCount", ref _variableCount, value); }
        }

        public string GetNewVariableName(string prefix)
        {
            return string.Format("{0}{1}", prefix, VariableCount++);
        }

        public override IEnumerable<IItem> Children
        {
            get
            {
                foreach (var item in NodeItems)
                {
                    if (item is SystemNode) yield return item;
                    if (item is ComponentNode) yield return item;
                    
                }
            }
        }
    }
}
