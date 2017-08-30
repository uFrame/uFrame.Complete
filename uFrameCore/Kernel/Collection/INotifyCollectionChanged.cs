namespace uFrame.Kernel.Collection
{
    #if !(NETFX_CORE || NET_4_6)
    public interface INotifyCollectionChanged
    {
        event NotifyCollectionChangedEventHandler CollectionChanged;
    }
    #endif
}