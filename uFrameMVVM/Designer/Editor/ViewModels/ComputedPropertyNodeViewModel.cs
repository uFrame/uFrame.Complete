namespace uFrame.MVVM {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using uFrame.Editor.Core;
    using uFrame.Editor.Graphs.Data;
    using uFrame.Editor.GraphUI.ViewModels;
    using uFrame.Editor.Platform;
    using uFrame.Editor.TypesSystem;
    using UnityEngine;
    
    
    public class ComputedPropertyNodeViewModel : ComputedPropertyNodeViewModelBase {

        public ComputedPropertyNodeViewModel(ComputedPropertyNode graphItemObject, uFrame.Editor.GraphUI.ViewModels.DiagramViewModel diagramViewModel) :
            base(graphItemObject, diagramViewModel)
        {
        }
    }
}
