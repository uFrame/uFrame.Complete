using System.IO;

namespace uFrame.Editor.Compiling.CodeGen
{
    public abstract class FileGeneratorBase
    {
        public string SystemPath { get; set; }
        public string AssetPath { get; set; }
        public abstract string CreateOutput();
        public override string ToString()
        {
            return CreateOutput();
        }
        public abstract bool CanGenerate(FileInfo fileInfo);
        
    }
}