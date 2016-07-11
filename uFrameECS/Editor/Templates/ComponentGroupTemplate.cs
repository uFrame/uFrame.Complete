using UnityEngine;
using uFrame.Editor.Compiling.CodeGen;

namespace uFrame.ECS.Templates
{
    public partial class ComponentGroupTemplate
    {
        [ForEach("Components"), GenerateProperty, WithField(typeof(_ITEMTYPE_), typeof(SerializeField))]
        public _ITEMTYPE_ _Name_ { get; set; }

    }
}