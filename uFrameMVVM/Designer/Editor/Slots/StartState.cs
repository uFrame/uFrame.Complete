namespace uFrame.MVVM {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using uFrame.Editor.Database.Data;
    using uFrame.Editor.Graphs.Data;
    
    
    public class StartState : StartStateBase {
    }
    
    public partial interface IStartStateConnectable : uFrame.Editor.Graphs.Data.IDiagramNodeItem, uFrame.Editor.Graphs.Data.IConnectable {
    }
}
