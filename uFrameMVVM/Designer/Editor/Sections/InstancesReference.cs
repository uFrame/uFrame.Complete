using System.Text.RegularExpressions;
using uFrame.Editor.Core;
using uFrame.Editor.Database.Data;
using uFrame.Editor.Graphs.Data;
using uFrame.Json;

namespace uFrame.MVVM
{
    public class InstancesReference : InstancesReferenceBase
    {
        private string _name = string.Empty;

        [JsonProperty]
        public override string Name {
            get { return _name; }
            set
            {
                if (this.Repository != null)
                {
                    this.Changed("Name", ref _name, value);
                }
                else
                {
                    _name = value;
                }
            }
        }

        public override bool AllowInputs
        {
            get { return false; }
        }

        public override bool AllowOutputs
        {
            get { return false; }
        }
    }

    public partial interface IInstancesConnectable : IDiagramNodeItem, IConnectable
    {
    }
}
