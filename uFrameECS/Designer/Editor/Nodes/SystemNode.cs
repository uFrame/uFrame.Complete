using uFrame.Editor.Database.Data;
using uFrame.Editor.Graphs.Data;
using uFrame.Json;

namespace uFrame.ECS.Editor
{
    using System.Collections.Generic;
    using System.Linq;
    


    public class SystemNode : SystemNodeBase, IClassNode {

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
        public override bool AllowInputs
        {
            get { return false; }
        }

        public override bool AllowOutputs
        {
            get { return false; }
        }

        public override void Validate(List<ErrorInfo> errors)
        {

            base.Validate(errors);

           
        }
        
        public IEnumerable<HandlerNode> EventHandlers
        {
            get
            {
                
                return this.FilterNodes.OfType<HandlerNode>().OrderBy(p=>p.SetupOrder);
            }
        }
    }
    
    public partial interface ISystemConnectable : IDiagramNodeItem, IConnectable {
    }
}
