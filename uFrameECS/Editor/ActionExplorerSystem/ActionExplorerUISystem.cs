using uFrame.ECS.Editor;
using uFrame.Editor;
using uFrame.Editor.Core;
using uFrame.Editor.GraphUI;
using uFrame.Editor.GraphUI.Drawers;
using uFrame.Editor.GraphUI.Events;
using uFrame.Editor.Platform;
using uFrame.Editor.Unity;

namespace uFrame.ECS.Editor.ActionExplorerSystem
{
    using UnityEngine;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using UnityEditor;

    public class ActionExplorerUISystem : DiagramPlugin, IQueryDesignerWindowModalContent
    {
        private List<IItem> _dataItems;
        private TreeViewModel _actionsViewModel;
        private IPlatformDrawer _platformDrawer;
        private string _searchCriterial;

        public void QueryDesignerWindowModalContent(List<DesignerWindowModalContent> content)
        {
            //        content.Add(new DesignerWindowModalContent()
            //        {
            //            Drawer = DrawActionsExplorer
            //        });
        }


        public IPlatformDrawer PlatformDrawer
        {
            get { return _platformDrawer ?? (_platformDrawer = InvertApplication.Container.Resolve<IPlatformDrawer>()); }
            set { _platformDrawer = value; }
        }

        public List<IItem> DataItems
        {
            get
            {

                if (_dataItems == null)
                {
                    //                var items = new List<IEnumerable<QuickAccessItem>>(); 
                    //                _dataItems = new List<IItem>();
                    //                
                    //                Signal<IQuickAccessEvents>(_ => _.QuickAccessItemsEvents(new QuickAccessContext()
                    //                {
                    //                    ContextType = typeof(IInsertQuickAccessContext)
                    //                }, items));
                    //
                    //                _dataItems = items.SelectMany(s => s).ToList();
                    _dataItems = new List<IItem>();
                    var _categoryTitles = uFrameECS.Actions
                        .Where(_ => _.Value.Category != null)
                        .SelectMany(_ => _.Value.Category.Title)
                        .Distinct();


                    foreach (var categoryTitle in _categoryTitles)
                    {
                        var category = new DefaultTreeItem(categoryTitle, "");

                        category.Children =
                            uFrameECS.Actions.Values.Where(
                                _ => _.Category != null && _.Category.Title.Contains(categoryTitle)).ToArray();

                        _dataItems.Add(category);
                    }

                }

                return _dataItems;

            }
            set { _dataItems = value; }
        }

        public TreeViewModel ActionsViewModel
        {
            get
            {
                return _actionsViewModel ?? (_actionsViewModel = new TreeViewModel()
                    {
                        Data = DataItems.Cast<IItem>().ToList()
                    });
            }
            set { _actionsViewModel = value; }
        }

        public void DrawActionsExplorer(Rect obj)
        {
            if (ActionsViewModel.IsDirty) ActionsViewModel.Refresh();

            var mainContentBounds = obj.Pad(0, 0, 0, 30);
            var listRect = mainContentBounds.LeftHalf();
            var actionCode = mainContentBounds.RightHalf().BottomHalf();

            Signal<IDrawTreeView>(_ => _.DrawTreeView(listRect.PadSides(15), ActionsViewModel, (m, i) => { }));


            //var selectedAction = ActionsViewModel.SelectedData as ActionNode;

            var item = ActionsViewModel.SelectedData;
            if (item != null)
            {
                PlatformDrawer.DrawStretchBox(actionCode, CachedStyles.WizardSubBoxStyle, 15);
                PlatformDrawer.DrawLabel(actionCode.PadSides(15),
                    string.Format("Title: {0}\nType: {1}\n", item.Title, item.GetType().Name)
                    , CachedStyles.BreadcrumbTitleStyle, DrawingAlignment.TopLeft);
            }
            var updateButton = new Rect().WithSize(80, 24).InnerAlignWithBottomLeft(obj);



            PlatformDrawer.DoButton(updateButton, "Update", ElementDesignerStyles.ButtonStyle, () =>
            {
                EditorApplication.delayCall += () =>
                {
                    ActionsViewModel = null;
                    DataItems = null;
                };
            });


            PlatformDrawer.DrawTextbox("12345", new Rect().WithSize(120, 24).Align(updateButton).RightOf(updateButton), _searchCriterial, GUI.skin.textField,
                (a, b) =>
                {
                    if (_searchCriterial == a) return;
                    _searchCriterial = a;

                    if (!string.IsNullOrEmpty(_searchCriterial))
                    {
                        ActionsViewModel.Predicate = i =>
                        {
                            if (string.IsNullOrEmpty(i.Title)) return false;

                            if (
                                CultureInfo.CurrentCulture.CompareInfo.IndexOf(i.Title, _searchCriterial,
                                    CompareOptions.IgnoreCase) != -1) return true;

                            if (!string.IsNullOrEmpty(i.SearchTag) &&
                                CultureInfo.CurrentCulture.CompareInfo.IndexOf(i.SearchTag, _searchCriterial,
                                    CompareOptions.IgnoreCase) != -1) return true;

                            return false;
                        };
                    }
                    else
                    {
                        ActionsViewModel.Predicate = null;
                    }
                    ActionsViewModel.IsDirty = true;
                });

        }

    }


    public class ActionExplorerWindow : EditorWindow
    {
        private ActionExplorerUISystem _system;


        public static void Init()
        {
            var window = ScriptableObject.CreateInstance<ActionExplorerWindow>();
            window.minSize = new Vector2(200, 200);
            window.Show();
            window.Repaint();
            window.Focus();
        }


        public ActionExplorerUISystem System
        {
            get { return _system ?? (_system = InvertApplication.Container.Resolve<ActionExplorerUISystem>()); }
            set { _system = value; }
        }

        void OnGUI()
        {
            System.DrawActionsExplorer(new Rect(0, 0, this.position.width, this.position.height));

        }

        void Update()
        {
            Repaint();
        }

    }
}