using System;

namespace uFrame.Editor.Core
{
    public interface IDebugLogger
    {
        void Log(string message);
        void LogException(Exception ex);
    }
}