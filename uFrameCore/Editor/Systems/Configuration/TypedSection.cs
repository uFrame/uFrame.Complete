namespace uFrame.Editor.Configurations
{
    public class TypedSection : Section
    {
        public bool AllowNoneType { get; set; }

        public TypedSection(string name, SectionVisibility visibility) : base(name, visibility)
        {
        }
    }
}