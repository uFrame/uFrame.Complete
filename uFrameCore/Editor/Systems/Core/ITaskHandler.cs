using System.Collections;

namespace uFrame.Editor.Core
{
    public interface ITaskHandler
    {
        void BeginTask(IEnumerator task);
        void BeginBackgroundTask(IEnumerator task);
    }
}