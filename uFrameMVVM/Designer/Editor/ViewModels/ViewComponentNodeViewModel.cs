namespace uFrame.MVVM {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using uFrame.Editor.Graphs.Data;
    using uFrame.Editor.GraphUI.ViewModels;
    using uFrame.Editor.Platform;
    
    
    public class ViewComponentNodeViewModel : ViewComponentNodeViewModelBase {
        
        public ViewComponentNodeViewModel(ViewComponentNode graphItemObject, uFrame.Editor.GraphUI.ViewModels.DiagramViewModel diagramViewModel) : 
                base(graphItemObject, diagramViewModel) {
        }
    }
}
