using uFrame.ECS.Editor;
using uFrame.Editor.GraphUI.ViewModels;

namespace uFrame.ECS.Editor
{ 
    public class ModuleNodeViewModel : ModuleNodeViewModelBase {
        
        public ModuleNodeViewModel(ModuleNode graphItemObject, DiagramViewModel diagramViewModel) : 
                base(graphItemObject, diagramViewModel) {
        }
    }
}
