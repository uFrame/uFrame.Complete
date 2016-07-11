using uFrame.Editor.Database.Data;
using uFrame.IOC;

namespace uFrame.Editor
{
    public class RepoService : DiagramPlugin
    {
        
        public IRepository Repository
        {
            get { return Container.Resolve<IRepository>(); }
        }

        public override void Initialize(UFrameContainer container)
        {
            base.Initialize(container);

        }

        public override void Loaded(UFrameContainer container)
        {
            base.Loaded(container);
        }
    }
}