using System;
using uFrame.ECS.APIs;
using uFrame.ECS.UnityUtilities;
using UniRx;

namespace uFrame.ECS.Components
{
    /// <summary>
    /// The base class for all group items, for example ReactiveGroup`TGroupItem`
    /// </summary>
    public class GroupItem : IEcsComponent
    {
        private Entity _entityView;
        private CompositeDisposable _disposer;
        /// <summary>
        /// Is this component enabled
        /// </summary>
	    public bool Enabled
	    {
		    get{return true;}
		    set{
		    	
		    }
	    }

        public bool IsDirty
        {
            get { return false; }
            set { }
        }

        /// <summary>
        /// The entity id for the entity this group item belongs to
        /// </summary>
        public int EntityId { get; set; }

        public virtual int ComponentId
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        /// <summary>
        /// The entity object that this groupitem belongs to
        /// </summary>
        public Entity Entity
        {
            get { return _entityView ?? (_entityView = EntityService.GetEntityView(EntityId)); }
        }

        public CompositeDisposable Disposer
        {
            get { return _disposer ?? (_disposer = new CompositeDisposable()); }
            set { _disposer = value; }
        }
    }
}