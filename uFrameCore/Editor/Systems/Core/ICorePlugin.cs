using System;
using uFrame.IOC;

namespace uFrame.Editor.Core
{
    public interface ICorePlugin
    {
        string Title { get; }
        bool Enabled { get; set; }
        decimal LoadPriority { get; }
        string PackageName { get; }
        bool Required { get; }
        bool Ignore { get; }
        UFrameContainer Container { get; set; }
        TimeSpan InitializeTime { get; set; }
        TimeSpan LoadTime { get; set; }
        void Initialize(UFrameContainer container);
        void Loaded(UFrameContainer container);
        
    }
}