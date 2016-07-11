using uFrame.Editor.Core;

namespace uFrame.Editor.Database
{
    public interface IGraphConfiguration : IItem
    {
        string CodeOutputPath { get;  }
        string Namespace { get; set; }
        bool IsCurrent { get; set; }
        string FullPath { get;  }
    }
}