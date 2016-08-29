using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using uFrame.ECS.APIs;
using uFrame.ECS.Components;
using uFrame.Kernel;
using UniRx;
using UnityEngine;

namespace uFrame.ECS.Systems
{
    public class BlackBoardGroup : DescriptorGroup<IBlackBoardComponent>
    {
        
    }
    public class BlackBoardSystem : EcsSystem, IBlackBoardSystem
    {

        public override void KernelLoading()
        {
            base.KernelLoading();
            // Make sure all the blackboard components are registered first.
            StartingBlackBoardComponents = uFrameKernel.Instance.gameObject.GetComponentsInChildren<IBlackBoardComponent>();
        }

        public IBlackBoardComponent[] StartingBlackBoardComponents { get; set; }

        public override void Setup()
        {
            base.Setup();
            BlackBoards = this.ComponentSystem.RegisterGroup<BlackBoardGroup, IBlackBoardComponent>();
        }

        public BlackBoardGroup BlackBoards { get; set; }

        public bool Has<TType>() where TType : class
        {
            return BlackBoards.Components.OfType<TType>().Any();
        }

        public TType Get<TType>() where TType : class
        {
            var item = BlackBoards.Components.OfType<TType>().FirstOrDefault();
                //?? StartingBlackBoardComponents.OfType<TType>().FirstOrDefault();
            return item ?? EcsComponent.CreateObject(typeof(TType)) as TType;
        }

        public TType EnsureBlackBoard<TType>() where TType : class, IEcsComponent
        {
            return Get<TType>();
        }
    }
    public interface IBlackBoardComponent : IEcsComponent
    {
        
    }
}