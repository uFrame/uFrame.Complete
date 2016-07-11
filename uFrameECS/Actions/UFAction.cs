using System.Collections;
using uFrame.ECS.Components;
using uFrame.ECS.Systems;
using UnityEngine.EventSystems;

namespace uFrame.ECS.Actions
{
    public abstract class UFAction
    {
        public Entity EntityView;
        public EcsSystem System;

        public virtual void Execute()
        {
            
        }

        public virtual IEnumerator Perform()
        {
            Execute();
            return null;
        }
        
    }



    public static class ActionExtensions
    {
        public static IEnumerator ExecuteAction(this EcsSystem system, IEnumerator actionMethod)
        {
       
            if (actionMethod != null)
            {
                // Move through each item of the routine
                while (actionMethod.MoveNext())
                {
                    // Intercept actions
                    var current = actionMethod.Current;
                    var action = current as UFAction;

                    if (action != null)
                    {
                        var actionExecute = system.ExecuteAction(action.Perform());
                        if (actionExecute != null)
                        {
                            while (actionExecute.MoveNext())
                            {
                                yield return actionExecute.Current;
                            }
                        }
                    }
                    else
                    {
                        // Return it as normal
                        yield return current;
                    }
                 
                }
            }
        }

        public static void ExecuteHandler(this EcsSystem system, IEnumerator handlerMethod)
        {
            system.StartCoroutine(ExecuteAction(system, handlerMethod));
        }

    }
}