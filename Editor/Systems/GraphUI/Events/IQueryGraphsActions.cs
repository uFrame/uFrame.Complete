using System.Collections.Generic;
using uFrame.Editor.Wizards.Data;

namespace uFrame.Editor.GraphUI.Events
{
    public interface IQueryGraphsActions
    {
        void QueryGraphsAction(List<ActionItem> items);
    }
}