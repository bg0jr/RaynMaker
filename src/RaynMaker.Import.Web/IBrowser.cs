using System.Collections.Generic;
using RaynMaker.Import.Spec;

namespace RaynMaker.Import.Web
{
    public interface IBrowser
    {
        void Navigate( string url );

        void LoadDocument( IEnumerable<NavigatorUrl> urls );
    }
}
