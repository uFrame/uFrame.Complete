using System;
using System.CodeDom;
using uFrame.Editor.Compiling.CodeGen;
using uFrame.Editor.Graphs.Data;

namespace uFrame.MVVM.Templates
{
    public class CreateBindingSignatureParams
    {
        private CodeTypeDeclaration _context;

        private Func<Type, CodeTypeReference> _convertGenericParameter;

        private ViewNode _elementView;

        private ITypedItem _sourceItem;

        private string _subscribablePropertyNameFormat;

        public CodeTypeDeclaration Context
        {
            get
            {
                return this._context;
            }
        }

        public Func<Type, CodeTypeReference> ConvertGenericParameter
        {
            get
            {
                return this._convertGenericParameter;
            }
        }

        public ViewNode ElementView
        {
            get
            {
                return this._elementView;
            }
        }

        public ITypedItem SourceItem
        {
            get
            {
                return this._sourceItem;
            }
        }

        public string SubscribablePropertyNameFormat
        {
            get
            {
                return this._subscribablePropertyNameFormat;
            }
        }

        public TemplateContext<ViewNode> Ctx
        {
            get;
            set;
        }

        public bool DontImplement
        {
            get;
            set;
        }

        public BindingsReference BindingsReference
        {
            get;
            set;
        }

        public CreateBindingSignatureParams(CodeTypeDeclaration context, Func<Type, CodeTypeReference> convertGenericParameter, ViewNode elementView, ITypedItem sourceItem, string subscribablePropertyNameFormat = "{0}Property")
        {
            this._context = context;
            this._convertGenericParameter = convertGenericParameter;
            this._elementView = elementView;
            this._sourceItem = sourceItem;
            this._subscribablePropertyNameFormat = subscribablePropertyNameFormat;
        }
    }
}
