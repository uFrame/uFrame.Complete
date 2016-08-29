using System;

namespace uFrame.MVVM.ViewModels
{
    public class ViewModelCommandInfo
    {
        public ISignal Signal { get; private set; }

        public string Name { get; set; }

        public Type ParameterType { get; set; }


        public ViewModelCommandInfo(string name, ISignal signal)
        {
            Signal = signal;
            Name = name;
        }
    }
}
