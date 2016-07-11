using System;
using System.Linq;
using uFrame.Editor.Core;
using uFrame.Editor.Graphs.Data;
using uFrame.Editor.GraphUI.Drawers;
using uFrame.Editor.GraphUI.ViewModels;
using uFrame.Editor.Platform;
using UnityEngine;

namespace uFrame.Architect.Editor.Data
{
    public class ShellNodeConfigDrawer : DiagramNodeDrawer<ShellNodeConfigViewModel>, IInspectorDrawer
    {
        public ShellNodeConfigDrawer(ShellNodeConfigViewModel viewModel)
            : base(viewModel)
        {
        }

        public override void Draw(IPlatformDrawer platform, float scale)
        {
            base.Draw(platform, scale);

            if (IsSelected)
            {
                var selectedChild = Children.Skip(1).FirstOrDefault(p => p.IsSelected);
                var width = 85f;
                var buttonHeight = 25;
                var toolbarRect = new Rect(this.Bounds.x - width - 4, this.Bounds.y + 8, width, selectedChild == null ? (buttonHeight * 3) + 20 : (buttonHeight * 4) + 20);

                platform.DrawStretchBox(toolbarRect, CachedStyles.WizardSubBoxStyle, 12f);
                toolbarRect.y += 10;

                var x = toolbarRect.x;
                var y = toolbarRect.y;

                if (selectedChild != null)
                {
                    platform.DoButton(new Rect(x, y, toolbarRect.width, buttonHeight), "Remove", CachedStyles.WizardListItemBoxStyle,
                        () =>
                        {
                            NodeViewModel.RemoveSelected();
                        });
                    y += buttonHeight;
                }
                platform.DoButton(new Rect(x, y, toolbarRect.width, buttonHeight), "+ Add Section", CachedStyles.WizardListItemBoxStyle,
                    () =>
                    {
                        ShowAddPointerMenu<ShellNodeConfigSection>("Section", () =>
                        {
                            NodeViewModel.AddSectionItem();
                        }, _ => { NodeViewModel.AddSectionPointer(_); });
                    });
                y += buttonHeight;
                platform.DoButton(new Rect(x, y, toolbarRect.width, buttonHeight), "+ Input", CachedStyles.WizardListItemBoxStyle,
                    () =>
                    {
                        ShowAddPointerMenu<ShellNodeConfigInput>("Input", () =>
                        {
                            NodeViewModel.AddInputItem();
                        }, _ => { NodeViewModel.AddInputPointer(_); });
                    });
                y += buttonHeight;
                platform.DoButton(new Rect(x, y, toolbarRect.width, buttonHeight), "+ Output", CachedStyles.WizardListItemBoxStyle,
                    () =>
                    {
                        ShowAddPointerMenu<ShellNodeConfigOutput>("Output", () =>
                        {
                            NodeViewModel.AddOutputItem();
                        }, _ => { NodeViewModel.AddOutputPointer(_); });

                    });
                y += buttonHeight;



            }


        }

        private void ShowAddPointerMenu<TItem>(string name, Action addItem, Action<TItem> addPointer) where TItem : IDiagramNodeItem
        {



            var ctxMenu = new UnityEditor.GenericMenu();
            ctxMenu.AddItem(new GUIContent("New " + name), false,
                () => { InvertApplication.Execute(() => { addItem(); }); });
            ctxMenu.AddSeparator("");
            var nodeConfigSection =
                NodeViewModel.DiagramViewModel.CurrentRepository.AllOf<TItem>();
            foreach (var item in nodeConfigSection)
            {
                var item1 = item;
                ctxMenu.AddItem(new GUIContent(item.Name), false,
                    () => { InvertApplication.Execute(() => { addPointer(item1); }); });
            }
            ctxMenu.ShowAsContext();

        }

        protected override void DrawChildren(IPlatformDrawer platform, float scale)
        {
            for (int index = 0; index < Children.Count; index++)
            {
                var item = Children[index];

                if (index == 0)
                {
                    item.Draw(platform, scale);
                    continue;
                }
                var optionsBounds = new Rect(item.Bounds.x, item.Bounds.y + 4, item.Bounds.width,
                    item.Bounds.height);
                if (item.IsSelected)
                {
                    platform.DrawStretchBox(optionsBounds, CachedStyles.Item1, 0f);
                }
                optionsBounds.width -= 35;
                //optionsBounds.x += 15;
                item.Draw(platform, scale);
                platform.DoButton(optionsBounds, "", CachedStyles.ClearItemStyle, () =>
                {
                    ViewModel.DiagramViewModel.DeselectAll();
                    ViewModel.Select();
                    item.ViewModelObject.Select();
                    InvertApplication.SignalEvent<IGraphSelectionEvents>(_ => _.SelectionChanged(item.ViewModelObject));
                });
            }
        }

        public void DrawInspector(IPlatformDrawer platformDrawer)
        {
#if UNITY_EDITOR

#endif
        }
    }
}