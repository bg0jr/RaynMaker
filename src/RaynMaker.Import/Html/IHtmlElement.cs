using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RaynMaker.Import.Html
{
    public interface IHtmlElement
    {
        IHtmlDocument Document{ get; }

        IHtmlElement Parent { get; }
        
        IEnumerable<IHtmlElement> Children { get; }

        string TagName { get; }

        string GetAttribute( string attr );

        string InnerText { get; }

        string InnerHtml { get;  }
    }
}
