namespace uFrame.Kernel.Collection
{
     #if !(NETFX_CORE || NET_4_6)
    public enum NotifyCollectionChangedAction
    {
        Reset,
        Add,
        Move,
        Remove,
        Replace
    }
#endif
}