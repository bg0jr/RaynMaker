using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RaynMaker.Import.Html
{
    public interface IHtmlDocument
    {
        Uri Url { get; }

        IHtmlElement Body { get; }
    }
}
