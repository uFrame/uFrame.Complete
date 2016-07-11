using System;
using uFrame.Editor.Core;
using uFrame.Editor.Attributes;

namespace uFrame.Editor.Graphs.Commands
{
    public class CreateGraphCommand : Command
    {
        [InspectorProperty]
        public string Name { get; set; }
        public Type GraphType { get; set; }
    }
}