namespace uFrame.Editor.Graphs.Data
{
    public interface ISelectable : IGraphItem
    {
        bool IsSelected { get; set; }
    }
}