using System.Collections.Generic;
using UnityEngine;

namespace uFrame.MVVM
{
    public static class MvcExtensions
    {
        public static T GetComponentFromInterface<T>(this MonoBehaviour behaviour) where T : class
        {
            return behaviour.gameObject.GetComponentFromInterface<T>();
        }

        public static T GetComponentFromInterface<T>(this GameObject gameObj) where T : class
        {
            foreach (Component component in gameObj.GetComponents<Component>())
            {
                if (component is T)
                {
                    T _ = component as T;
                    return _;
                }
            }
            return default(T);
        }

        public static IEnumerable<T> GetComponentsInDirectChildren<T>(this Transform tfm) where T : Component
        {
            for (int i = 0; i < tfm.childCount; i++)
            {
                var cmp = tfm.GetChild(i).GetComponent<T>();
                if (cmp == null) continue;
                yield return cmp;
            }
        }
    }
}