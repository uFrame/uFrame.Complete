using UnityEngine.UI;

namespace Example
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using uFrame.Kernel;
    using uFrame.Kernel.Serialization;
    using uFrame.MVVM;
    using uFrame.MVVM.Bindings;
    using uFrame.MVVM.Services;
    using uFrame.MVVM.ViewModels;
    using UniRx;
    using UnityEngine;


    public class LevelSelectScreenView : LevelSelectScreenViewBase
    {
        /// <summary>
        /// A prefab which represents level information (LevelListItem)
        /// </summary>
        public GameObject LevelListItemPrefab;

        /// <summary>
        /// Container which stores LevelListItems
        /// </summary>
        public RectTransform LevelListContainer;

        public override void AvailableLevelsOnAdd(LevelDescriptor levelDescriptor)
        {
            /*
             * We use naming convention: we name LevelListItems using LevelDescriptor.LevelScene property,
             * as it is unique for every level.
             * 
             * Before actually creating a new LevelListItem, we ensure that it is not already in the list
             */
            var item = LevelListContainer.FindChild(levelDescriptor.LevelScene);
            if (item != null) return;

            /* Instantiate new LevelListItem */
            var go = Instantiate(LevelListItemPrefab) as GameObject;
            item = go.transform;

            /* Parent created LevelListItem to the container */
            item.SetParent(LevelListContainer);

            /* 
             * Each LevelListItem has similar hierarchy. We can use it and setup different objects and their
             * values, based on the LevelDescriptor 
             */
            item.FindChild("LevelTitle").GetComponent<Text>().text = levelDescriptor.Title;
            item.FindChild("LevelDescription").GetComponent<Text>().text = levelDescriptor.Description;

            /* Setup the name based on LevelDescriptor.LevelScene */
            item.gameObject.name = levelDescriptor.LevelScene;

            /* Make button interactable, if level is unlocked */
            item.GetComponent<Button>().interactable = !levelDescriptor.IsLocked;

            /* Make button scale 1,1,1 in case unity overrides it during instantiation */
            item.GetComponent<Button>().transform.localScale = Vector3.one;

            /* 
             * MOST IMPORTANT: attach unique handler to this button, 
             * which executes command with a specific LevelDescriptor 
             */
            this.BindButtonToHandler(item.GetComponent<Button>(), () =>
            {
                ExecuteSelectLevel(levelDescriptor);
            });
        }

        public override void AvailableLevelsOnRemove(LevelDescriptor levelDescriptor)
        {
            /* Simply remove visual representation of LevelDescriptor, if it ever exited */
            var item = LevelListContainer.FindChild(levelDescriptor.LevelScene);
            if (item != null) Destroy(item.gameObject);
        }

        protected override void InitializeViewModel(uFrame.MVVM.ViewModels.ViewModel model)
        {
            base.InitializeViewModel(model);
            // NOTE: this method is only invoked if the 'Initialize ViewModel' is checked in the inspector.
            // var vm = model as LevelSelectScreenViewModel;
            // This method is invoked when applying the data from the inspector to the viewmodel.  Add any view-specific customizations here.
        }

        public override void Bind()
        {
            base.Bind();
            // Use this.LevelSelectScreen to access the viewmodel.
            // Use this method to subscribe to the view-model.
            // Any designer bindings are created in the base implementation.
        }
    }
}
