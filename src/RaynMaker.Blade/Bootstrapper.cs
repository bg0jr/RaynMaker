using System.ComponentModel.Composition.Hosting;
using Microsoft.Practices.Prism.Interactivity;
using Plainion.AppFw.Wpf;
using Plainion.Prism.Interactivity;

namespace RaynMaker.Blade
{
    class Bootstrapper : BootstrapperBase<Shell>
    {
        protected override void ConfigureAggregateCatalog()
        {
            base.ConfigureAggregateCatalog();

            AggregateCatalog.Catalogs.Add( new AssemblyCatalog( GetType().Assembly ) );
            AggregateCatalog.Catalogs.Add( new AssemblyCatalog( typeof( PopupWindowActionRegionAdapter ).Assembly ) );
        }

        protected override Microsoft.Practices.Prism.Regions.RegionAdapterMappings ConfigureRegionAdapterMappings()
        {
            var mappings = base.ConfigureRegionAdapterMappings();
            mappings.RegisterMapping( typeof( PopupWindowAction ), Container.GetExportedValue<PopupWindowActionRegionAdapter>() );
            return mappings;
        }
    }
}
