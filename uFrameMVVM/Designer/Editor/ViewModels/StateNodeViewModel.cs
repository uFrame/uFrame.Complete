namespace uFrame.MVVM {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using uFrame.Editor.Graphs.Data;
    using uFrame.Editor.GraphUI.ViewModels;
    using uFrame.Editor.Platform;
    
    
    public class StateNodeViewModel : StateNodeViewModelBase {

        public bool IsCurrentState;


        public StateNodeViewModel(StateNode graphItemObject, uFrame.Editor.GraphUI.ViewModels.DiagramViewModel diagramViewModel) : 
                base(graphItemObject, diagramViewModel) {
        }
    }
}
