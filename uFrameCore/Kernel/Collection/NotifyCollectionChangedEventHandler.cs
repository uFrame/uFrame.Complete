using System;
using System.Collections.Specialized;

namespace uFrame.Kernel.Collection
{
    #if !NETFX_CORE
    public delegate void NotifyCollectionChangedEventHandler(Object sender, NotifyCollectionChangedEventArgs changeArgs);
    #endif
}