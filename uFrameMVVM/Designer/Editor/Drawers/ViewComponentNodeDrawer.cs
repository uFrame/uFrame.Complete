namespace uFrame.MVVM {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using uFrame.Editor.GraphUI.Drawers;
    
    
    public class ViewComponentNodeDrawer : GenericNodeDrawer<ViewComponentNode,ViewComponentNodeViewModel> {
        
        public ViewComponentNodeDrawer(ViewComponentNodeViewModel viewModel) : 
                base(viewModel) {
        }
    }
}
