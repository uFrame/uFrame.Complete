using System;
using System.CodeDom;
using uFrame.Editor.Graphs.Data;

namespace uFrame.MVVM.Templates
{
    public class BindingHandlerArgs
    {
        public CodeMemberMethod Method
        {
            get;
            set;
        }

        public ViewNode View
        {
            get;
            set;
        }

        public ElementNode Element
        {
            get
            {
                return this.View.Element;
            }
        }

        public ITypedItem SourceItem
        {
            get;
            set;
        }

        public CodeTypeDeclaration Decleration
        {
            get;
            set;
        }

        public bool IsDesignerFile
        {
            get;
            set;
        }
    }
}
