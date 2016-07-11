using System.Collections.Generic;
using uFrame.Editor.Core;
using uFrame.Editor.GraphUI;
using uFrame.Editor.GraphUI.Drawers;
using uFrame.Editor.Unity;
using uFrame.Editor.Wizards.Data;
using uFrame.Editor.Wizards.Events;
using UnityEditor;
using UnityEngine;

namespace uFrame.Editor
{
    public class DatabaseListWindow : EditorWindow
    {
        private List<DatabasesListItem> _items;
        public List<DatabasesListItem> Items
        {
            get
            {
                if (_items == null)
                {
                    _items = new List<DatabasesListItem>();
                    InvertApplication.SignalEvent<IQueryDatabasesListItems>(_=>_.QueryDatabasesListItems(_items));
                }
                return _items;
            }
            set { _items = value; }
        }
        //[MenuItem("Window/uFrame/Databases")]
        public static void Init()
        {
            var window = GetWindow<DatabaseListWindow>();

            window.Repaint();
            window.Show();

        }

        void OnGUI()
        {
            var bounds = new Rect(0, 0, this.position.width, this.position.height);
            var platform = InvertGraphEditor.PlatformDrawer;

            InvertApplication.SignalEvent<IDrawDatabasesList>(_=>
            {
                _.DrawDatabasesList(platform,bounds,Items);
            });

            platform.DoButton(new Rect().WithSize(80,30).InnerAlignWithBottomLeft(bounds.PadSides(15)),"Close",ElementDesignerStyles.ButtonStyle,Close);

        }

    }
}
