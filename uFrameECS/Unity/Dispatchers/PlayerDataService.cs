using System.Linq;
using uFrame.Attributes;
using uFrame.ECS.Systems;
using uFrame.Kernel;
using UniRx;

namespace uFrame.ECS.UnityUtilities
{
    public class PlayerDataService : EcsSystem, IPlayerDataService
    {
        public ComponentRepositoryBehaviour Repository;
        /// <summary>
        /// Should the data be loaded without the need to explicitly publishing the "Load" event.
        /// If AutoLoad is on, when a persistant component is loaded it will load the data.
        /// </summary>
        public bool AutoLoad = true;

        public override void Setup()
        {
            base.Setup();
            PlayerDataGroup = this.ComponentSystem.RegisterGroup<PlayerDataGroup, IPlayerDataComponent>();
            if (AutoLoad)
            {
                PlayerDataGroup.CreatedObservable
                    .Subscribe(Repository.LoadComponent)
                    .DisposeWith(this);
            }
            this.OnEvent<SavePlayerData>()
                .Subscribe(Save)
                .DisposeWith(this);

            this.OnEvent<LoadPlayerData>()
                .Subscribe(Load)
                .DisposeWith(this);

        }

        public PlayerDataGroup PlayerDataGroup { get; set; }
        public override void Loaded()
        {
            base.Loaded();
            if (AutoLoad)
            {
                Load(new LoadPlayerData());
            }
        }

        public virtual void Save(SavePlayerData data)
        {
            foreach (var item in PlayerDataGroup.Components.Where(p => p.IsDirty))
            {
                Repository.SaveComponent(item);
            }
        }

        public virtual void Load(LoadPlayerData data)
        {
            foreach (var item in PlayerDataGroup.Components)
            {
                Repository.LoadComponent(item);
            }
        }
    }


}