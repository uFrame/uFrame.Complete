using System;
using System.Collections.Generic;
using uFrame.ECS.Components;

namespace uFrame.ECS.APIs
{
    /// <summary>
    /// Manages components of a specific type
    /// </summary>
    public interface IEcsComponentManager : IGroup
    {
        int ComponentId { get; set; }
        Type For { get; }
        IEnumerable<IEcsComponent> All { get; }
        void RegisterComponent(IEcsComponent item);
        void UnRegisterComponent(IEcsComponent item);
        IEcsComponent ForEntity(int entityId);

    }
}