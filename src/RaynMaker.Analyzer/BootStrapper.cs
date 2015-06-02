using System.ComponentModel.Composition.Hosting;
using System.IO;
using Plainion.AppFw.Wpf;
using Plainion.AppFw.Wpf.ViewModels;
using RaynMaker.Infrastructure;

namespace RaynMaker.Analyzer
{
     class Bootstrapper : BootstrapperBase<Shell>
    {
        protected override void ConfigureAggregateCatalog()
        {
            base.ConfigureAggregateCatalog();

            AggregateCatalog.Catalogs.Add( new AssemblyCatalog( GetType().Assembly ) );

            AggregateCatalog.Catalogs.Add( new TypeCatalog(
                typeof( ProjectLifecycleViewModel<Project> ),
                typeof( TitleViewModel<Project> )
                ) );

            var moduleRoot = Path.GetDirectoryName( GetType().Assembly.Location );
            foreach( var moduleFile in Directory.GetFiles( moduleRoot, "RaynMaker.*.dll" ) )
            {
                AggregateCatalog.Catalogs.Add( new AssemblyCatalog( moduleFile ) );
            }
        }
    }
}
