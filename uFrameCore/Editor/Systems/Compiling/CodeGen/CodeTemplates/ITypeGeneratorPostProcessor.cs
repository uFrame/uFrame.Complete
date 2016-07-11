using System;
using System.CodeDom;

namespace uFrame.Editor.Compiling.CodeGen
{
    public interface ITypeGeneratorPostProcessor
    {
        Type For { get; }
        CodeGenerator Generator { get; set; }
        CodeTypeDeclaration Declaration { get; set; }
        
        void Apply();
    }
}