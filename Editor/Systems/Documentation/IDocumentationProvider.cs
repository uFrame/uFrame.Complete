using System.Collections.Generic;

namespace uFrame.Editor.Documentation
{
    public interface IDocumentationProvider
    {
        void GetDocumentation(IDocumentationBuilder node);
        void GetPages(List<DocumentationPage> rootPages);

    }
}