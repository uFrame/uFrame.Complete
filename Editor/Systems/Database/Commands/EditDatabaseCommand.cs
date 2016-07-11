using uFrame.Editor.Attributes;
using uFrame.Editor.Core;
using uFrame.Editor.Database.Data;

namespace uFrame.Editor.Database.Commands
{
    public class EditDatabaseCommand : Command
    {
        public uFrameDatabaseConfig Configuration { get; set; }

        [InspectorProperty("A namespace to be used for all the generated classes.")]
        public string Namespace { get; set; }

        [InspectorProperty("A path for the generated code output.")]
        public string CodePath { get; set; }


    }
}