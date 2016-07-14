using System;
using uFrame.Attributes;
using UnityEngine.UI;

namespace uFrame.ECS.Actions
{
    [ActionLibrary, uFrameCategory("uGUI")]
    public static class uGUILibrary
    {
        [ActionTitle("Set Label")]
        public static void SetLabel(string text)
        {
            
        }
    }

    [ActionTitle("Set Text Label")]
    public partial class SetNumericDisplayText : UFAction
    {

        [In("Label")]
        public Text Label;

        [In("text")]
        public String text;

        public override void Execute()
        {
            base.Execute();
            Label.text = text;
        }
    }

}