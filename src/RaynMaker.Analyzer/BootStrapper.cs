using System.ComponentModel.Composition.Hosting;
using Plainion.AppFw.Wpf;

namespace RaynMaker.Analyzer
{
     class Bootstrapper : BootstrapperBase<Shell>
    {
        protected override void ConfigureAggregateCatalog()
        {
            base.ConfigureAggregateCatalog();

            AggregateCatalog.Catalogs.Add( new AssemblyCatalog( GetType().Assembly ) );
        }
    }
}
