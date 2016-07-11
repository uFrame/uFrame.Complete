using uFrame.Editor.Compiling.CommonNodes;
using uFrame.Editor.Graphs.Data;
using uFrame.Editor.GraphUI.Drawers;
using uFrame.Editor.GraphUI.ViewModels;
using uFrame.IOC;

namespace uFrame.Editor
{
    public class NotesPlugin : DiagramPlugin
    {
        public override decimal LoadPriority
        {
            get { return 1000; }
        }

        public override void Initialize(UFrameContainer container)
        {
            base.Initialize(container);
         
            container.AddNode<NoteNode, NoteNodeViewModel, NoteNodeDrawer>("Note");
            container.AddNode<ImageNode, ImageNodeViewModel, ImageNodeDrawer>("Image");
            
            foreach (var node in FilterExtensions.AllowedFilterNodes)
            {
                node.Value.Add(typeof(NoteNode));       
                node.Value.Add(typeof(ImageNode));       
            }
        }
    }
}
