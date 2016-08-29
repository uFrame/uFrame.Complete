namespace uFrame.MVVM {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using uFrame.Editor.GraphUI.Drawers;
    
    
    public class StateMachineNodeDrawer : GenericNodeDrawer<StateMachineNode,StateMachineNodeViewModel> {
        
        public StateMachineNodeDrawer(StateMachineNodeViewModel viewModel) : 
                base(viewModel) {
        }
    }
}
