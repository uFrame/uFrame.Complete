using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using uFrame.Common;
using uFrame.ECS.Editor;
using uFrame.Editor.Compiling.Commands;
using uFrame.Editor.Core;
using uFrame.Editor.Database.Data;
using uFrame.Editor.GraphUI;
using uFrame.Editor.Input;
using uFrame.Editor.Menus;
using uFrame.Editor.NavigationSystem;
using uFrame.Editor.Platform;
using uFrame.Editor.Unity;

namespace uFrame.Editor.DebugSystem
{
    public class DebugEvent : Command
    {
        public string ActionId { get; set; }
        public object[] Variables { get; set; }
        public int Result { get; set; }
    }
    public interface IBreakpointHit
    {
        void BreakpointHit();
    }
    public class DebugSystem : DiagramPlugin,
        IExecuteCommand<DebugEvent>,
        IExecuteCommand<ContinueCommand>,
        IExecuteCommand<StepCommand>,
        IExecuteCommand<ToggleBreakpointCommand>,
        IDataRecordRemoved,
        IContextMenuQuery,
        IToolbarQuery //,
        //IDrawInspector
    {

        

        private Dictionary<string, Breakpoint> _breakpoints;

        public Dictionary<string, Breakpoint> Breakpoints
        {
            get { return _breakpoints ?? (Container.Resolve<IRepository>().All<Breakpoint>().ToDictionary(p => p.ForIdentifier)); }
            set { _breakpoints = value; }
        }

        public void Execute(ToggleBreakpointCommand command)
        {
            if (command.Action.BreakPoint == null)
            {
                var breakPoint = new Breakpoint
                {
                    ForIdentifier = command.Action.Identifier
                };
                Container.Resolve<IRepository>().Add(breakPoint);
            }
            else
            {
                Container.Resolve<IRepository>().Remove(command.Action.BreakPoint);
            }
            _breakpoints = null;
        }

        public void QueryContextMenu(ContextMenuUI ui, MouseEvent evt, params object[] obj)
        {
            var actionVM = obj.FirstOrDefault() as SequenceItemNodeViewModel;
            if (actionVM != null)
            {
                ui.AddCommand(new ContextMenuItem()
                {
                    Title = "Breakpoint",
                    Group = "Debug",
                    Checked = actionVM.SequenceNode.BreakPoint != null,
                    Command = new ToggleBreakpointCommand()
                    {
                        Action = actionVM.SequenceNode,

                    }
                });
            }
        }

        public static SequenceItemNode CurrentBreakpoint { get; set; }
        public bool ShouldStep { get; set; }
        public void Execute(DebugEvent command)
        {
            //LastDebugEvent = command;
            //if (CurrentBreakpoint != null && CurrentBreakpoint.Identifier == command.ActionId)
            //{
            //    if (ShouldContinue == true)
            //    {
            //        command.Result = 0;
            //        CurrentBreakpoint.IsSelected = false;
            //        ShouldContinue = false;
            //        CurrentBreakpoint = null;
            //        return;
            //    }
            //    command.Result = 1;
            //    return;
            //}

            //if (Breakpoints.ContainsKey(command.ActionId))
            //{
            //    command.Result = 1;
            //    Signal<IBreakpointHit>(_ => _.BreakpointHit());
            //    CurrentBreakpoint = Breakpoints[command.ActionId].Action;
            //    Execute(new NavigateToNodeCommand()
            //    {
            //        Node = CurrentBreakpoint
            //    });

            //}
            //else if (ShouldStep)
            //{
            //    CurrentBreakpoint = Container.Resolve<IRepository>().GetById<ActionNode>(command.ActionId);
            //    command.Result = 1;
            //    Execute(new NavigateToNodeCommand()
            //    {
            //        Node = CurrentBreakpoint
            //    });
            //    ShouldStep = false;
            //}
        }

        public static DebugInfo LastDebugEvent { get; set; }

        public void QueryToolbarCommands(ToolbarUI ui)
        {
            if (CurrentBreakpoint != null)
            {
                ui.AddCommand(new ToolbarItem()
                {
                    Command = new ContinueCommand(),
                    Title = "Continue"
                });
                ui.AddCommand(new ToolbarItem()
                {
                    Command = new StepCommand(),
                    Title = "Step"
                });
            }
            ui.AddCommand(new ToolbarItem()
            {
                Title = "Debug Mode",
                Checked = IsDebugMode,
                Description = "Enabling debug mode will turn on breakpoints, which you can setup on Action nodes and other Sequence nodes.",
                Command = new LambdaCommand("Debug Mode", () =>
                {
                    IsDebugMode = !IsDebugMode;
                    InvertApplication.Execute(new SaveAndCompileCommand()
                    {
                        ForceCompileAll = true
                    });
                }),
                Position = ToolbarPosition.Right
            });

        }

        public bool ShouldContinue;
        public void Execute(ContinueCommand command)
        {
            ShouldContinue = true;
            ShouldStep = false;
       

        }

        public override bool Enabled
        {
            get
            {
#if DEMO
                return false;
#endif
                return base.Enabled;

            }
            set { base.Enabled = value; }
        }

        public void Execute(StepCommand command)
        {
            ShouldContinue = true;
            ShouldStep = true;
            LastActionId = LastDebugEvent.ActionId;
        }

        public void DrawInspector(Rect rect)
        {
            if (LastDebugEvent != null)
            {
                foreach (var obj in LastDebugEvent.Variables)
                {
                    if (GUIHelpers.DoToolbarEx(obj.GetType().ToString()))
                    {
                        var properties = obj.GetType().GetFields(BindingFlags.Default | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                        foreach (var property in properties)
                        {
                            var val = property.GetValue(obj);
                            if (val != null)
                            {
                                EditorGUILayout.LabelField(property.Name, val.ToString());
                            }
                        }
                    }

                }
            }
        }

        public string LastActionId;

        //public void OnActionExecuting(DebugInfo command)
        //{
        //    if (Breakpoints.ContainsKey(command.ActionId))
        //    {
        //        while (!ShouldContinue)
        //        {
        //            ElementsDesigner.Instance.Repaint();
        //            EditorApplication.Step();
                    
                  
        //        }
        //    }
        //}

        public void OnActionExecuting(DebugInfo command)
        {
            // If we are at the breakpoint
            if (CurrentBreakId != null && CurrentBreakId == command.ActionId)
            {
                if (ShouldContinue == true)
                {
                    command.Result = 0;
                    if (!ShouldStep)
                    {
                        CurrentBreakId = null;
                    }
                    
                    return;
                }
                command.Result = 1;
                return;
            }

            if (CurrentBreakId != null && CurrentBreakId == command.PreviousId && command.PreviousId != null)
            {
              
                if (ShouldStep)
                {
                    CurrentBreakId = command.ActionId;
                    command.Result = 1;
                    ShouldContinue = false;
                    Execute(new NavigateToNodeCommand()
                    {
                        Node = Container.Resolve<IRepository>().GetById<SequenceItemNode>(command.ActionId)
                    });
             
                } else
                if (ShouldContinue)
                {
                    ShouldContinue = false;
                    command.Result = 0;
                    CurrentBreakId = null;
                }
              
                   
               
                return;
            }

            if (Breakpoints.ContainsKey(command.ActionId))
            {


                CurrentBreakId = command.ActionId;
                command.Result = 1;
                Execute(new NavigateToNodeCommand()
                {
                    Node = Container.Resolve<IRepository>().GetById<SequenceItemNode>(command.ActionId)
                });
             

            }






            //if (CurrentBreakId == command.ActionId)
            //{
            //    if (ShouldContinue)
            //    {
            //        command.Result = 0;
            //    }
            //    else
            //    {
            //        command.Result = 1;
            //    }
            //}
            //else
            //{
            //    ShouldContinue = false;



            //}


            LastDebugEvent = command;
            //if (Breakpoints.ContainsKey(command.ActionId))
            //{
            //    command.Result = 1;
            //    Signal<IBreakpointHit>(_ => _.BreakpointHit());
            //    CurrentBreakpoint = Breakpoints[command.ActionId].Action;
            //    Execute(new NavigateToNodeCommand()
            //    {
            //        Node = CurrentBreakpoint
            //    });

            //}
            //else if (ShouldStep)
            //{
            //    if (CurrentBreakpoint != null && command.PreviousId == LastActionId)
            //    {
            //        CurrentBreakpoint = Container.Resolve<IRepository>().GetById<ActionNode>(command.ActionId);
            //        command.Result = 1;
            //        Execute(new NavigateToNodeCommand()
            //        {
            //            Node = CurrentBreakpoint
            //        });
            //        ShouldStep = false;

            //    }

            //}

        }

        public static string CurrentBreakId { get; set; }
        public static bool IsDebugMode
        {
            get
            {
                if (InvertGraphEditor.Prefs == null) return false; // Testability
                return InvertGraphEditor.Prefs.GetBool("UFRAME_DEBUG_MODE", true);
            }
            set { InvertGraphEditor.Prefs.SetBool("UFRAME_DEBUG_MODE", value); }
        }

        public void RecordRemoved(IDataRecord record)
        {
            var srecord = record as SequenceItemNode;

            if (srecord != null && srecord.BreakPoint != null)
            {
                Container.Resolve<IRepository>().Remove(srecord.BreakPoint);
            }
        }
    }

    public class ToggleBreakpointCommand : Command
    {
        public SequenceItemNode Action { get; set; }
    }

}
