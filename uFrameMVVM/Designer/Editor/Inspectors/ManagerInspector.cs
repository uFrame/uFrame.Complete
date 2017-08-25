using System;
using UnityEditor;
using UnityEngine;

namespace uFrame.MVVM.Editor {
    public abstract class ManagerInspector<TManaged> : uFrameInspector {
        private string _compileCompleteCallback;
        private string _createNewText;

        //    private bool _createOpen = false;

        public bool _open = false;

        private bool _shouldGeneratePrefabNow = false;

        public Type ManagedType {
            get {
                return typeof(TManaged);
            }
        }

        public Component Target {
            get {
                return target as Component;
            }
        }

        public void OnCompileComplete() {
            OnCompileComplete();
        }
        
        public void Update() {
            if (_shouldGeneratePrefabNow) {
                if (EditorApplication.isCompiling) {
                    return;
                }

                _shouldGeneratePrefabNow = false;
                this.GetType().GetMethod(_compileCompleteCallback).Invoke(this, null);
                //_compileCompleteCallback = null;
            }
        }

        public void WaitForCompileToComplete(string complete = "OnCompileComplete") {
            _compileCompleteCallback = complete;
            AssetDatabase.Refresh();
            _shouldGeneratePrefabNow = true;
        }


        protected abstract bool ExistsInScene(Type itemType);

        protected abstract string GetTypeNameFromName(string name);
    }
}