using uFrame.Kernel;
using UnityEngine;

namespace uFrame.ECS.UnityUtilities
{
    public static class EcsSystemLoaderExtensions
    {
        public static TSystemType AddSystem<TSystemType>(this SystemLoader t) where TSystemType : Component, ISystemService
        {
            
            var parent = uFrameKernel.Instance.transform;
            var existing = parent.GetComponentInChildren<TSystemType>();
            if (existing != null)
            {
                return existing;
            }
            var go = new GameObject(typeof(TSystemType).Name);
            go.transform.parent = parent;
            return go.AddComponent<TSystemType>();
        }
    }
}