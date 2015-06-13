using System;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Windows;
using Microsoft.Practices.Prism.Interactivity;
using Microsoft.Practices.Prism.Regions;
using Plainion.AppFw.Wpf;
using Plainion.AppFw.Wpf.ViewModels;
using Plainion.Prism.Interactivity;
using RaynMaker.Analyzer.Services;
using RaynMaker.Infrastructure;
using RaynMaker.Infrastructure.Controls;

namespace RaynMaker.Analyzer
{
    class Bootstrapper : BootstrapperBase<Shell>
    {
        protected override void ConfigureAggregateCatalog()
        {
            base.ConfigureAggregateCatalog();

            AggregateCatalog.Catalogs.Add( new AssemblyCatalog( GetType().Assembly ) );
            AggregateCatalog.Catalogs.Add( new AssemblyCatalog( typeof( PopupWindowActionRegionAdapter ).Assembly ) );

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

        protected override Microsoft.Practices.Prism.Regions.RegionAdapterMappings ConfigureRegionAdapterMappings()
        {
            var mappings = base.ConfigureRegionAdapterMappings();
            mappings.RegisterMapping( typeof( OverlayViewAction ), Container.GetExportedValue<OverlayViewActionRegionAdapter>() );
            mappings.RegisterMapping( typeof( PopupWindowAction ), Container.GetExportedValue<PopupWindowActionRegionAdapter>() );
            return mappings;
        }

        public override void Run( bool runWithDefaultConfiguration )
        {
            Application.Current.DispatcherUnhandledException += Current_DispatcherUnhandledException;

            base.Run( runWithDefaultConfiguration );

            // we have to call this here in order to support regions which are provided by modules
            RegionManager.UpdateRegions();
        }

        void Current_DispatcherUnhandledException( object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e )
        {
            MessageBox.Show( e.Exception.ToString(), "Unhandled exception", MessageBoxButton.OK, MessageBoxImage.Error );
            e.Handled = true;
        }
    }
}
