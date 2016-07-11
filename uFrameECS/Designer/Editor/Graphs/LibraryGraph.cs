using uFrame.ECS.Editor;
using uFrame.Editor.Database.Data;
using uFrame.Json;

namespace uFrame.ECS.Editor
{
    public class LibraryGraph : LibraryGraphBase, IVariableNameProvider {

        private int _variableCount;


        [JsonProperty]
        public int VariableCount
        {
            get { return _variableCount; }
            set { this.Changed("VariableCount", ref _variableCount, value); }
        }

        public string GetNewVariableName(string prefix)
        {
            return string.Format("{0}{1}", prefix, VariableCount++);
        }
    }
}
