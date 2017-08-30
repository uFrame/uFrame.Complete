using System;
using System.Collections;
using uFrame.Editor.Core;

namespace uFrame.Editor.Unity
{
    public class TaskSystem : DiagramPlugin, IUpdate, ITaskHandler
    {
        private const int MaxLagMilliseconds = 50;
        public IEnumerator Task { get; set; }
        public void Update()
        {
            var updateStarted = DateTime.Now;

            while (Task != null)
            {
                var passed = (DateTime.Now - updateStarted).TotalMilliseconds;
                if (!IsModal && passed > MaxLagMilliseconds) break;
                //if (passed > MaxLagMilliseconds) break;

                if (!Task.MoveNext())
                {

                    Signal<ITaskProgressEvent>(_ => _.Progress(0f, string.Empty, IsModal));

                    Task = null;
                }
                else
                {
                    var current = Task.Current as TaskProgress;
                    if (current != null)
                    {
                        Signal<ITaskProgressEvent>(_ => _.Progress(current.Percentage, current.Message, IsModal));
                    }
                }


            }


        }
        public bool IsModal { get; set; }
        public void BeginTask(IEnumerator task)
        {
            Task = task;
            IsModal = true;
        }

        public void BeginBackgroundTask(IEnumerator task)
        {
            if (Task != null && IsModal) return;
            IsModal = false;
            Task = task;
        }
    }
}