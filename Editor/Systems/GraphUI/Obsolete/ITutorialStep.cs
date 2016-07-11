using System;
using uFrame.Editor.Documentation;

namespace uFrame.Editor.GraphUI
{
    public interface ITutorialStep
    {
        string Name { get; set; }
        Action DoIt { get; set; }
        Func<string> IsDone { get; set; }
        Action<IDocumentationBuilder> StepContent { get; set; }
        string IsComplete { get; set; }
    }
}