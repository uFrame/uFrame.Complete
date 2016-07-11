using uFrame.Editor.Attributes;
using uFrame.Editor.Graphs.Data;
using uFrame.Editor.Graphs.Data.Types;
using uFrame.Json;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace uFrame.MVVM
{
    public class CommandsChildItem : CommandsChildItemBase, IMemberInfo
    {
        [InspectorProperty]
        public bool Publish
        {
            get
            {
                return base["Publish"];
            }
            set
            {
                base["Publish"] = value;
            }
        }

        public CommandNode OutputCommand
        {
            get
            {
                return this.RelatedTypeNode as CommandNode;
            }
        }

        public override bool AllowInputs
        {
            get
            {
                return false;
            }
        }

        public string ClassName
        {
            get
            {
                return this.Name + "Command";
            }
        }

        public override string DefaultTypeName
        {
            get
            {
                return typeof(void).FullName;
            }
        }

        [JsonProperty]
        public override string Name
        {
            get
            {
                var oc = OutputCommand;
                if(oc != null)
                {
                    return oc.Name;
                }
                return base.Name;
            }
            set
            {
                base.Name = value;
            }
        }

        public override void RemoveType()
        {
            base.RemoveType();
            this.RelatedType = typeof(void).FullName;
        }

        public override void BeginEditing()
        {
            if(this.OutputCommand != null)
            {
                base.IsEditing = false;
                return;
            }
            base.BeginEditing();
        }

        public override bool CanOutputTo(IConnectable input)
        {
            if (this.OutputTo<IClassTypeNode>() != null)
            {
                return false;
            }
            if (input is HandlersReference) return false;
            return base.CanOutputTo(input);
        }

        public bool HasArgument
        {
            get
            {
                return !string.IsNullOrEmpty(this.RelatedTypeName) && !this.RelatedTypeName.Contains("Void");
            }
        }

        public override void Validate(List<ErrorInfo> errors)
        {
            base.Validate(errors);
            var otherCommand = Node.Graph.AllGraphItems.OfType<CommandsChildItem>()
                                         .FirstOrDefault(p => p != this && p.Name == this.Name && p.OutputCommand == null);
            if (otherCommand != null)
            {

                errors.AddError(string.Format("The command {0} is already being used on node {1}.", this.Name, otherCommand.Node.Name), 
                                this,
                                () => { Name = this.Node.Name + this.Name; });
            }
        }
    }

    public partial interface ICommandsConnectable : IDiagramNodeItem, IConnectable
    {
    }
}
