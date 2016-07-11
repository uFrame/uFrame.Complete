using uFrame.Editor.Attributes;
using uFrame.Editor.Configurations;
using uFrame.Editor.Database.Data;
using uFrame.Editor.Graphs.Data;
using uFrame.Json;

namespace uFrame.ECS.Editor
{
    using System.Collections.Generic;

    public class ConditionNode : ConditionNodeBase {
        private PropertyIn _ainput;
        private PropertyIn _binput;
        private ConditionComparer _comparer;

        [InputSlot("A")]
        public PropertyIn AInput
        {
            get
            {
                return _ainput ?? (_ainput = new PropertyIn()
                {
                  
                    Node = this.Node,
                    Name = "A",
                    DoesAllowInputs = true,
                    Identifier = Identifier + ":" + "A",
                      Repository = this.Repository,
                });
            }
        }

        [NodeProperty,JsonProperty]
        public ConditionComparer Comparer
        {
            get { return _comparer; }
            set { this.Changed("Comparer", ref _comparer, value); }
        }

        [InputSlot("B")]
        public PropertyIn BInput
        {
            get
            {
                return _binput ?? (_binput = new PropertyIn()
                {
                   
                    Node = this.Node,
                    Name = "B",
                    DoesAllowInputs = true,
                    Identifier = Identifier + ":" + "B",
                     Repository = this.Repository,
                });
            }
        }
        
        public override string GetExpression()
        {
            return AInput.Item.ValueExpression + Sign + BInput.Item.ValueExpression;
        }

        public string Sign
        {
            get
            {
                switch (Comparer)
                {
                    case ConditionComparer.Equal:
                        return "==";
                        case ConditionComparer.GreaterThen:
                        return ">";
                        case ConditionComparer.GreaterThenOrEqual:
                        return ">=";
                        case ConditionComparer.LessThen:
                        return "<";
                        case ConditionComparer.LessThenOrEqual:
                        return "<=";
                        case ConditionComparer.NotEqual:
                        return "!=";
                }
                return "==";
            }
        }

        public override void Validate(List<ErrorInfo> errors)
        {
            base.Validate(errors);
            if (AInput.Item == null)
                errors.AddError("A input is required.",this);
            if (BInput.Item == null)
                errors.AddError("B input is required.", this);
        }

        public override IEnumerable<IGraphItem> GraphItems
        {
            get
            {
                yield return AInput;
                yield return BInput;
            }
        }

        public override IEnumerable<NodeInputConfig> SlotConfigurations
        {
            get { return base.SlotConfigurations; }
        }
    }

    public enum ConditionComparer
    {
        Equal,
        NotEqual,
        GreaterThen,
        GreaterThenOrEqual,
        LessThen,
        LessThenOrEqual
        
    }
    
    public partial interface IConditionConnectable : IDiagramNodeItem, IConnectable {
    }
}
