using System;

namespace uFrame.Editor.Compiling.CodeGen
{
    [Flags]
    public enum TemplateLocation
    {
        DesignerFile = 0,
        EditableFile = 1,
        Both = 2
    }
}