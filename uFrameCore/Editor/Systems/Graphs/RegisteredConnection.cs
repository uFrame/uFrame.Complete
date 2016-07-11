using System;
using uFrame.Editor.Core;
using uFrame.Editor.Graphs.Data;

namespace uFrame.Editor.Graphs
{
    public class RegisteredConnection
    {
        public Type TOutputType { get; set; }
        public Type TInputType { get; set; }

        public virtual bool CanConnect(IConnectable output, IConnectable input)
        {
            if (CanConnect(output.GetType(), input.GetType()))
            {
                if (output.GetType().Name == "ShellNodeConfig" && input.GetType().Name == "ShellNodeConfigInput")
                {
                    InvertApplication.Log("!!!!Bingo!!!!");
                    InvertApplication.Log("CanOutputTo : " + output.CanOutputTo(input));
                    InvertApplication.Log("CanInputFrom : " + input.CanInputFrom(output));
                }
                    
                if (output.CanOutputTo(input) && input.CanInputFrom(output))
                {
                    return true;
                }
            }
            return false;
        }
        public bool CanConnect(Type output, Type input)
        {
            if (TOutputType.IsAssignableFrom(output))
            {
                if (TInputType.IsAssignableFrom(input))
                {
                    return true;
                }
            }
            return false;
        }
    }
}