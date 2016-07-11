using System.Collections.Generic;
using System.Linq;
using uFrame.Common;
using uFrame.ECS.APIs;
using uFrame.ECS.Systems;
using uFrame.ECS.UnityUtilities;
using uFrame.Kernel;
using UniRx;
using UnityEngine;

namespace uFrame.ECS.UnityUtilities
{
    public class EcsSystemLoader : SystemLoader
    {
   
        private ISystemUpdate[] _items;
        private ISystemFixedUpdate[] _itemsFixed;

        public override void Load()
        {
            base.Load();
            Container.RegisterInstance<IBlackBoardSystem>(this.AddSystem<BlackBoardSystem>());
            Container.RegisterInstance<IComponentSystem>(this.AddSystem<EcsComponentService>());

        }

        public void Update()
        {
            if (uFrameKernel.IsKernelLoaded)
            {
                if (_items == null)
                {
                    _items = uFrameKernel.Instance.Services.OfType<ISystemUpdate>().ToArray();
                }

                for (int index = 0; index < _items.Length; index++)
                {
                    var item = _items[index];
                    item.SystemUpdate();
                }
            }
        }
        public void FixedUpdate()
        {
            if (uFrameKernel.IsKernelLoaded)
            {
                if (_itemsFixed == null)
                {
                    _itemsFixed = uFrameKernel.Instance.Services.OfType<ISystemFixedUpdate>().ToArray();
                }

                for (int index = 0; index < _itemsFixed.Length; index++)
                {
                    var item = _itemsFixed[index];
                    item.SystemFixedUpdate();
                }
            }
        }
    }


    public static class DebugService
    {
        private static Subject<DebugInfo> _debugInfo;


        public static IObservable<DebugInfo> DebugInfo
        {
            get { return _debugInfo ?? (_debugInfo = new Subject<DebugInfo>()); }
        }
        public static void NotifyDebug2(string previousActionId, string actionId, object[] variables)
        {
#if UNITY_EDITOR
            if (_debugInfo != null)
            {
                var debugInfo = new DebugInfo()
                {
                    PreviousId = previousActionId,
                    ActionId = actionId,
                    Variables = variables
                };
                _debugInfo.OnNext(debugInfo);

            }
#endif
        }
        public static int NotifyDebug(string previousActionId, string actionId, object[] variables)
        {
#if UNITY_EDITOR
            if (_debugInfo != null)
            {
                var debugInfo = new DebugInfo()
                {
                    PreviousId = previousActionId,
                    ActionId = actionId,
                    Variables = variables
                };
                _debugInfo.OnNext(debugInfo);
                return debugInfo.Result;
            }
#endif
            return 0;
        }
    }

}
public static class DebugExtensions
{
    public static int DebugInfo(this object obj,string previousId, string actionId, params object[] variables)
    {

        return DebugService.NotifyDebug(previousId, actionId, variables);

    }
    public static int DebugInfo(this object obj, string actionId, params object[] variables)
    {

        return DebugService.NotifyDebug(string.Empty, actionId, variables);

    }
    public static void DebugInfo2(this object obj, string previousId, string actionId, params object[] variables)
    {

        DebugService.NotifyDebug(string.Empty, actionId, variables);

    }
}