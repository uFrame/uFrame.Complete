using uFrame.Editor.Input;

namespace uFrame.Editor.Platform
{
    public interface IContextMenuQuery
    {
        void QueryContextMenu(ContextMenuUI ui, MouseEvent evt, params object[] obj);
    }
}