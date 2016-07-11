using System;
using uFrame.Attributes;
using uFrame.ECS.APIs;
using uFrame.Kernel;
using UniRx;

namespace uFrame.ECS.Actions
{
    [ActionTitle("Interval"), uFrameCategory("Timers"), AsyncAction, ActionDescription("Repeat something over time with an certain interval.")]
    public class Interval : UFAction
    {
        [In, Description("How much to wait in minutes before next invocation. Will be added to Seconds.")]
        public int Minutes;
        [In]
        public int Seconds;

        [In, Description("An component to use as a disposer for the timer.")]
        public IEcsComponent DisposeWith;

        [Out, Description("Connect to the next action. It will be invoked over time with a certain interval.")]
        public Action Tick;

        [Out, Description("An object to dispose, to stop the timer.")]
        public IDisposable Result;

        public override void Execute()
        {
           
            Result = Observable.Interval(new TimeSpan(0, 0, Minutes, Seconds, 0)).Subscribe(_ =>
            {
                Tick();
            }).DisposeWith(System);
            if (DisposeWith != null)
            {
                Result.DisposeWith(DisposeWith);
            }
        }
    }

    [ActionTitle("Interval By Seconds"), uFrameCategory("Timers"), AsyncAction]
    public class IntervalBySeconds : UFAction
    {

        [In]
        public float Seconds;

        [In]
        public IEcsComponent DisposeWith;

        [Out]
        public Action Tick;

        [Out]
        public IDisposable Result;

        public override void Execute()
        {

            Result = Observable.Interval(TimeSpan.FromSeconds(Seconds)).Subscribe(_ =>
            {
                Tick();
            }).DisposeWith(System);
            if (DisposeWith != null)
            {
                Result.DisposeWith(DisposeWith);
            }
        }
    }
}