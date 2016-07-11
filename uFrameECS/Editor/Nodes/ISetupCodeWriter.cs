using uFrame.Editor.Compiling.CodeGen;

namespace uFrame.ECS.Editor
{
    public interface ISetupCodeWriter
    {
        void WriteSetupCode(ISequenceVisitor visitor, TemplateContext ctx);
    }
}