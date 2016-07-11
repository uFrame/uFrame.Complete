namespace uFrame.Architect.Editor.Data
{
    public interface IShellSlotType : IShellNodeConfigItem
    {
        bool IsOutput { get; set; }
        bool AllowMultiple { get; set; }

        bool AllowSelection { get; set; }
    }
}