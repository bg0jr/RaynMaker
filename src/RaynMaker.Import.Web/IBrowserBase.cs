using System.Collections.Generic;
using RaynMaker.Import.Html;
using RaynMaker.Import.Spec;

namespace RaynMaker.Import.Web
{
    public interface IBrowserBase
    {
        void Navigate( string url );

        IHtmlDocument LoadDocument( IEnumerable<NavigatorUrl> urls );
    }
}
