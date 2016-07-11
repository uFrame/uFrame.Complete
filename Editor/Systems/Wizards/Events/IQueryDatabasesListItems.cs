using System.Collections.Generic;
using uFrame.Editor.Wizards.Data;

namespace uFrame.Editor.Wizards.Events
{
    public interface IQueryDatabasesListItems
    {
        void QueryDatabasesListItems(List<DatabasesListItem> items);
    }
}
