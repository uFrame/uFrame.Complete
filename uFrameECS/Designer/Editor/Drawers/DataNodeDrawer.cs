using uFrame.ECS.Editor;
using uFrame.Editor.GraphUI.Drawers;

namespace uFrame.ECS.Editor
{
    public class DataNodeDrawer : GenericNodeDrawer<DataNode,DataNodeViewModel> {
        
        public DataNodeDrawer(DataNodeViewModel viewModel) : 
                base(viewModel) {
        }
    }
}
