namespace uFrame.MVVM.ViewModels
{
    public interface IViewModelCommand
    {
        ViewModel Sender { get; set; }
    }
}