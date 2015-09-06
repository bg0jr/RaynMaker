using System;
using System.Collections.Generic;
using System.Windows.Forms;
using RaynMaker.Import.Html;
using RaynMaker.Import.Spec;

namespace RaynMaker.Import.Web
{
    public interface IBrowser
    {
        void Navigate( string url );

        IHtmlDocument LoadDocument( IEnumerable<NavigatorUrl> urls );

        event Action<Uri> Navigating;

        event Action<HtmlDocument> DocumentCompleted;
    }
}
