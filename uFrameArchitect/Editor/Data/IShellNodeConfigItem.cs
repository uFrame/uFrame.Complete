using System.Collections.Generic;
using uFrame.Editor.Attributes;
using uFrame.Editor.Configurations;
using uFrame.Editor.Documentation;
using uFrame.Editor.Graphs.Data;
using uFrame.Json;

namespace uFrame.Architect.Editor.Data
{
    public interface IShellNodeConfigItem : IDocumentable, IClassTypeNode
    {
        [JsonProperty, InspectorProperty]
        int Row { get; set; }
        [JsonProperty, InspectorProperty]
        int Column { get; set; }
        [InspectorProperty, JsonProperty]
        SectionVisibility Visibility { get; set; }
        string ReferenceClassName { get; }
        //string ClassName { get; }
        IEnumerable<IShellNodeConfigItem> IncludedInSections { get; }
        string TypeName { get; set; }
    }
}