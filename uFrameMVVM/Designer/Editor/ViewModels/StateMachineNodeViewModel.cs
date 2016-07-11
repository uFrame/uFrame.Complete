namespace uFrame.MVVM {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using uFrame.Editor.Graphs.Data;
    using uFrame.Editor.GraphUI.ViewModels;
    using uFrame.Editor.Platform;
    
    
    public class StateMachineNodeViewModel : StateMachineNodeViewModelBase {
        
        public StateMachineNodeViewModel(StateMachineNode graphItemObject, uFrame.Editor.GraphUI.ViewModels.DiagramViewModel diagramViewModel) : 
                base(graphItemObject, diagramViewModel) {
        }
    }
}
