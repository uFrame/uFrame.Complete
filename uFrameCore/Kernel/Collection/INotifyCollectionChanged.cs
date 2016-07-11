namespace uFrame.Kernel.Collection
{
    #if !NETFX_CORE
    public interface INotifyCollectionChanged
    {
        event NotifyCollectionChangedEventHandler CollectionChanged;
    }
    #endif
}