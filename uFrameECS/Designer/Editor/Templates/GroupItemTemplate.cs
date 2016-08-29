using System.CodeDom;
using uFrame.Editor.Compiling.CodeGen;

namespace uFrame.ECS.Templates
{
    public partial class GroupItemTemplate
    {
        [ForEach("SelectComponents"),GenerateProperty, WithField]
        public _ITEMTYPE_ _Name_ { get; set; }


        [GenerateProperty, AsOverride]
        public int ComponentId
        {
            get
            {
                Ctx._("return {0}", Ctx.Data.ComponentId);
                Ctx.CurrentProperty.Attributes = MemberAttributes.Override | MemberAttributes.Public;
                return 0;
            }
        }
    }
}