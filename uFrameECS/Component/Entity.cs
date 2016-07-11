using uFrame.ECS.APIs;
using uFrame.ECS.UnityUtilities;
using uFrame.Kernel;
using UnityEngine;

namespace uFrame.ECS.Components
{
    public partial class Entity : uFrameComponent, IEcsComponent
    {

        public Entity ProxyFor;

        [SerializeField]
        private int _entityId;

        public int EntityId
        {
            get
            {
                if (ProxyFor != null)
                {
                    return ProxyFor.EntityId;
                }
                return _entityId == 0 ? (_entityId = EntityService.NewId()) : _entityId;
            }
            set { _entityId = value; }
        }
		/// <summary>
        /// Is this component enabled
        /// </summary>
	    public bool Enabled
	    {
		    get
		    {
			    return this.enabled;
		    }
		    set { this.enabled = value; }
	    }

        public bool IsDirty
        {
            get { return false; }
            set
            {
                
            }
        }

        public int ComponentId
        {
            get { return 0; }
        }

        public override void KernelLoading()
        {
            base.KernelLoading();
            if (ProxyFor == null)
            EntityService.RegisterEntityView(this);

        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            if (ProxyFor == null)
            EntityService.UnRegisterEntityView(this);
        }
    }
}