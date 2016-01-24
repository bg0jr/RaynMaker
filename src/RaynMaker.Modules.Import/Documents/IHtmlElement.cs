using System.Collections.Generic;

namespace RaynMaker.Modules.Import.Documents
{
    public interface IHtmlElement
    {
        IHtmlDocument Document { get; }

        IHtmlElement Parent { get; }

        IReadOnlyList<IHtmlElement> Children { get; }

        string Id { get; }

        string TagName { get; }

        string GetAttribute( string attr );

        void SetAttribute( string attr, string value );

        string InnerText { get; }

        string InnerHtml { get; }

        string Style { get; set; }
    }
}
