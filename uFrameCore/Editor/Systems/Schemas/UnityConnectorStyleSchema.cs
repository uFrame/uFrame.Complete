using uFrame.Editor.GraphUI.Drawers.Schemas;
using uFrame.Editor.GraphUI.ViewModels;
using uFrame.Editor.Unity;
using UnityEngine;

namespace uFrame.Editor.Schemas
{
    public class UnityConnectorStyleSchema : ConnectorStyleSchema
    {
        protected override object ConstructTexture(ConnectorSide side, ConnectorDirection direction, bool connected, Color tint = default(Color))
        {

            string iconBase = null;

            if (direction == ConnectorDirection.Input && connected) iconBase = _filledInputIconCode;
            if (direction == ConnectorDirection.Input && !connected) iconBase = _emptyInputIconCode;
            if (direction == ConnectorDirection.Output && connected) iconBase = _filledOutputIconCode;
            if (direction == ConnectorDirection.Output && !connected) iconBase = _emptyOutputIconCode;
            if (direction == ConnectorDirection.TwoWay && connected) iconBase = _emptyTwoWayIconCode;
            if (direction == ConnectorDirection.TwoWay && !connected) iconBase = _filledTwoWayIconCode;

            var baseTexture = ElementDesignerStyles.GetSkinTexture(iconBase);

            if (tint != default(Color))
            {
                baseTexture = baseTexture.Tint(tint);
            }

            switch (side)
            {
                default:
                    return baseTexture;
            }

            return null;
        }
    }

}
