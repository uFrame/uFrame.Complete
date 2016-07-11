using System;

namespace uFrame.Editor.Core
{
    public interface IEventManager
    {
        Action AddListener(object listener);
        void Signal(Action<object> obj);
    }
}