using uFrame.Editor.Compiling.CodeGen;
using uFrame.Architect.Editor.Data;

namespace uFrame.Architect.Editor.Generators
{
    public class ShellPluginClassGenerator : GenericNodeGenerator<ShellPluginNode>
    {
        public override void Initialize(CodeFileGenerator codeFileGenerator)
        {
            base.Initialize(codeFileGenerator);
            TryAddNamespace("System.IO");

        }
    }
}