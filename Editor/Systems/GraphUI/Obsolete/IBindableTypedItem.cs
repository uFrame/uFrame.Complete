using uFrame.Editor.Graphs.Data;

namespace uFrame.Editor.GraphUI
{
    public interface IBindableTypedItem :  ITypedItem
    {
        bool AllowEmptyRelatedType { get;  }
        string FieldName { get; }
        string NameAsChangedMethod { get;  }
        string ViewFieldName { get; }

       
       
    }
}