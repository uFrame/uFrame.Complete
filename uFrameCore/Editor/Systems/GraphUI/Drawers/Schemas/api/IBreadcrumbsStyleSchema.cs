using UnityEngine;

namespace uFrame.Editor.GraphUI.Drawers.Schemas
{
    public interface IBreadcrumbsStyleSchema
    {

        object GetIcon(string name, Color tint = default(Color));

    }
}