using System;
using uFrame.Attributes;
using uFrame.Kernel;
using UniRx;

namespace uFrame.ECS.Actions
{
    [ActionTitle("Timer"),uFrameCategory("Timers"), AsyncAction]
    public class Timer : UFAction
    {
        [In] public int Minutes;
        [In] public int Seconds;

        [Out] public Action Complete;
        public override void Execute()
        {
            Observable.Timer(new TimeSpan(0, 0, Minutes, Seconds, 0)).Subscribe(_ =>
            {
                Complete();
            }).DisposeWith(System);
        }
    }  
    
    [ActionTitle("Timer (Milliseconds)"),uFrameCategory("Timers")]
    public class MillitTimer : UFAction
    {
        [In] public int Milliseconds;

        [Out] public Action Complete;
        public override void Execute()
        {
            Observable.Timer(new TimeSpan(0, 0, 0, 0, Milliseconds)).Subscribe(_ =>
            {
                Complete();
            }).DisposeWith(System);
        }
    }
}