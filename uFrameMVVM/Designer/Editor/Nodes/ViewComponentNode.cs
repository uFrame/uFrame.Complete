namespace uFrame.MVVM
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using uFrame.Editor.Configurations;
    using uFrame.Editor.Core;
    using uFrame.Editor.Graphs.Data;


    public class ViewComponentNode : ViewComponentNodeBase
    {
        public ViewNode View
        {
            get
            {
                return this.InputFrom<ViewNode>();
            }
        }

        public override void Validate(List<ErrorInfo> errors)
        {
            base.Validate(errors);
            if(this.View == null)
            {
                errors.AddError(string.Format("View must be connected to the {0} ViewComponent.", this.Name), this);
            }
        }
    }

    public partial interface IViewComponentConnectable : uFrame.Editor.Graphs.Data.IDiagramNodeItem, uFrame.Editor.Graphs.Data.IConnectable
    {
    }
}
