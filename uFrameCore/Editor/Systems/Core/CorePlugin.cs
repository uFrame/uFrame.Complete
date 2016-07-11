using System;
using uFrame.IOC;

namespace uFrame.Editor.Core
{
    public abstract class CorePlugin : ICorePlugin
    {
        private UFrameContainer _container;

        public void Execute<TCommand>(TCommand command) where TCommand :  ICommand
        {
            InvertApplication.Execute(command);
        }
        public virtual string PackageName
        {
            get { return string.Empty; }
        }

        public virtual bool Required
        {
            get { return false; }
        }

        public virtual bool Ignore
        {
            get { return false; }
        }

        public virtual string Title
        {
            get { return this.GetType().Name; }
        }

        public abstract bool Enabled { get; set; }

        public virtual bool EnabledByDefault
        {
            get { return true; }
        }

        public virtual decimal LoadPriority { get { return 1; } }

        public TimeSpan InitializeTime { get; set; }
        public TimeSpan LoadTime { get; set; }
        
        public virtual void Initialize(UFrameContainer container)
        {
            Container = container;
        }

        public UFrameContainer Container
        {
            get { return InvertApplication.Container; }
            set { _container = value; }
        }

        public abstract void Loaded(UFrameContainer container);
    }
}