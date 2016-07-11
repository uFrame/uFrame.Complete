using uFrame.Editor.Configurations;
using uFrame.Editor.Core;
using uFrame.Editor.Database.Data;
using uFrame.Editor.Graphs.Data;
using uFrame.Editor.GraphUI.Scaffolding;
using uFrame.Editor.GraphUI.ViewModels;

namespace uFrame.ECS.Editor
{
    using System.Collections.Generic;


    public class HandlerNodeViewModel : HandlerNodeViewModelBase {
        public override bool IsEditable
        {
            get { return true; }
        }

        public HandlerNodeViewModel(HandlerNode graphItemObject, DiagramViewModel diagramViewModel) : 
                base(graphItemObject, diagramViewModel) {
        }

        public override NodeColor Color
        {
            get
            {
                return NodeColor.Indianred4;
            }
        }

        public HandlerNode Handler
        {
            get { return GraphItem as HandlerNode; }
        }

        public override IEnumerable<string> Tags
        {
            get
            {
                yield return Handler.DisplayName;

            }
        }

        //public override bool IsFilter
        //{
        //    get { return !Handler.CodeHandler; }
        //}

        public HandlerNode HandlerNode
        {
            get { return GraphItem as HandlerNode; }
        }

        public override string SubTitle
        {
            get
            {
                var meta = Handler.Meta as EventMetaInfo;
                if (meta != null && meta.Dispatcher && meta.SystemType != null)
                    return base.SubTitle + " with " + meta.SystemType.Name;
                return base.SubTitle;
            }
        }

        public override void PropertyChanged(IDataRecord record, string name, object previousValue, object nextValue)
        {
            base.PropertyChanged(record, name, previousValue, nextValue);
            if (record is PropertiesChildItem || record is CollectionsChildItem)
            {
                DataObjectChanged();
            }
        }

        protected override void CreateContent()
        {
            var inputs = Handler.HandlerInputs;
            if (IsVisible(SectionVisibility.WhenNodeIsNotFilter))
            {

                //if (inputs.Length > 0)
                //    ContentItems.Add(new GenericItemHeaderViewModel()
                //    {
                //        Name = "Mappings",
                //        DiagramViewModel = DiagramViewModel,
                //        IsNewLine = true,
                //    });



                foreach (var item in inputs)
                {
                    var vm = new InputOutputViewModel()
                    {
                        DataObject = item,
                        Name = item.Title,
                        IsInput = true,
                        IsOutput = false,
                        IsNewLine = true,
                        AllowSelection = true
                    };
                    ContentItems.Add(vm);


                }
            }
            else
            {
                foreach (var handlerIn in inputs)
                {
                    var handlerItem = handlerIn.Item;
                    if (handlerItem != null)
                    {
                        foreach (var component in handlerItem.SelectComponents)
                        {
                            var component1 = component;

                            ContentItems.Add(new GenericItemHeaderViewModel()
                            {
                                Name = component.Name,
                                IsBig = true,
                                IsNewLine = true,
                                NodeViewModel = this,
                            });
                            //ContentItems.Add(new GenericItemHeaderViewModel()
                            //{
                            //    Name = component.Name,
                            //    DataObject = component,
                            //    IsNewLine = true
                            //});
                            ContentItems.Add(new GenericItemHeaderViewModel()
                            {
                                Name = "Properties",
                                IsNewLine = true,
                                NodeViewModel = this,
                                //DataObject = component,
                                AddCommand = new LambdaCommand("", () =>
                                {
                                    var item = new PropertiesChildItem() { Node = component1 };
                                    DiagramViewModel.CurrentRepository.Add(item);
                                    item.Name = item.Repository.GetUniqueName("Collection");
                                    item.IsEditing = true;
                                    DataObjectChanged();
                                })
                                
                            });
                            foreach (var property in component.Properties)
                            {
                                ContentItems.Add(new ScaffoldNodeTypedChildItem<PropertiesChildItem>.ViewModel(property,this));
                            }
                            
                            ContentItems.Add(new GenericItemHeaderViewModel()
                            {
                                Name = "Collections",
                                IsNewLine = true,
                                NodeViewModel = this,
                                //DataObject = component,
                                AddCommand = new LambdaCommand("", () =>
                                {
                                    var item = new CollectionsChildItem {Node = component1};
                                    DiagramViewModel.CurrentRepository.Add(item);
                                    item.Name = item.Repository.GetUniqueName("Collection");
                                    item.IsEditing = true;
                                    DataObjectChanged();
                                })

                            });
                            foreach (var property in component.Collections)
                            {
                                ContentItems.Add(new ScaffoldNodeTypedChildItem<CollectionsChildItem>.ViewModel(property, this));

                            }

                        }
                    }
                }
            }
            base.CreateContent();
        }
    }
}
