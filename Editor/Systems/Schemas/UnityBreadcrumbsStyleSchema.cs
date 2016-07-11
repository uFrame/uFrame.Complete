using uFrame.Editor.GraphUI.Drawers.Schemas;
using uFrame.Editor.Unity;
using UnityEngine;

namespace uFrame.Editor.Schemas
{
    class UnityBreadcrumbsStyleSchema : BreadcrumbsStyleSchema
    {
        protected override object ConstructIcon(string name, Color tint = default(Color))
        {
            Texture2D texture = ElementDesignerStyles.GetSkinTexture(name);

            if (tint != default(Color))
            {
                texture = texture.Tint(tint);
            }

            return texture;
        }
    }
}
