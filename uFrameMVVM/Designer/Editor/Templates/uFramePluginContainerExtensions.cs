using UnityEngine;
using System;
using System.Collections;
using uFrame.IOC;
using System.Reflection;
using uFrame.Editor.Graphs.Data;

namespace uFrame.MVVM.Templates
{
    public static class uFramePluginContainerExtensions
    {
        public static uFrameBindingType AddBindingMethod(this IUFrameContainer container, Type type, MethodInfo method, Func<ITypedItem, bool> canBind)
        {
            return container.AddBindingMethod(new uFrameBindingType(type, method, canBind), method.Name);
        }

        public static uFrameBindingType AddBindingMethod(this IUFrameContainer container, Type type, string methodName, Func<ITypedItem, bool> canBind)
        {
            return container.AddBindingMethod(new uFrameBindingType(type, methodName, canBind), methodName);
        }

        public static uFrameBindingType AddBindingMethod(this IUFrameContainer container, uFrameBindingType info, string name)
        {
            container.RegisterInstance<uFrameBindingType>(info, name, true);
            return info;
        }
    }
}

