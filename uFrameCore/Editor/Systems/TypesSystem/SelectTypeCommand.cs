using System;
using uFrame.Editor.Core;
using uFrame.Editor.Graphs.Data;
using uFrame.Editor.Graphs.Data.Types;

namespace uFrame.Editor.TypesSystem
{
    public class SelectTypeCommand : Command
    {
        public bool AllowNone { get; set; }
        public bool PrimitiveOnly { get; set; }
        public bool IncludePrimitives { get; set; }
        public Action OnSelectionFinished { get; set; }
        public Predicate<ITypeInfo> Filter { get; set; }
        public ITypedItem Item { get; set; }
    }
}