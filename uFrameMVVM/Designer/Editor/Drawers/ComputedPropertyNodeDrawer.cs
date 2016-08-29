namespace uFrame.MVVM {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using uFrame.Editor.GraphUI.Drawers;
    
    
    public class ComputedPropertyNodeDrawer : GenericNodeDrawer<ComputedPropertyNode,ComputedPropertyNodeViewModel> {
        
        public ComputedPropertyNodeDrawer(ComputedPropertyNodeViewModel viewModel) : 
                base(viewModel) {
        }
    }
}
