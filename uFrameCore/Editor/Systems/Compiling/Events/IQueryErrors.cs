using System.Collections.Generic;
using uFrame.Editor.Graphs.Data;

namespace uFrame.Editor.Compiling.Events
{
    public interface IQueryErrors
    {
        void QueryErrors(List<ErrorInfo> errorInfo);
    }
}