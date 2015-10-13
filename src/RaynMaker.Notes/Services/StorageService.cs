using System.ComponentModel.Composition;
using System.IO;
using System.Windows;
using System.Windows.Documents;
using Microsoft.Practices.Prism.PubSubEvents;
using RaynMaker.Entities;
using RaynMaker.Infrastructure;
using RaynMaker.Infrastructure.Events;

namespace RaynMaker.Notes
{
    [Export]
    class StorageService
    {
        private IProjectHost myProjectHost;

        [ImportingConstructor]
        public StorageService( IProjectHost projectHost, IEventAggregator eventAggregator )
        {
            myProjectHost = projectHost;

            eventAggregator.GetEvent<CompanyDeletedEvent>().Subscribe( OnCompanyDeleted );
        }

        private void OnCompanyDeleted( string guid )
        {
            var file = GetFullPath( guid + ".rtf" );
            if( File.Exists( file ) )
            {
                File.Delete( file );
            }
        }

        public void Load( FlowDocument target )
        {
            var file = GetFullPath( "General.rtf" );
            if( !File.Exists( file ) )
            {
                return;
            }

            using( var stream = new FileStream( file, FileMode.Open, FileAccess.Read ) )
            {
                var range = new TextRange( target.ContentStart, target.ContentEnd );
                range.Load( stream, DataFormats.Rtf );
            }
        }

        private string GetFullPath( string fileName )
        {
            var root = Path.Combine( myProjectHost.Project.StorageRoot, "Notes" );
            if( !Directory.Exists( root ) )
            {
                Directory.CreateDirectory( root );
            }

            return Path.Combine( root, fileName );
        }

        public void Load( Stock stock, FlowDocument target )
        {
            var file = GetFullPath( stock.Company.Guid + ".rtf" );
            if( !File.Exists( file ) )
            {
                return;
            }

            using( var stream = new FileStream( file, FileMode.Open, FileAccess.Read ) )
            {
                var range = new TextRange( target.ContentStart, target.ContentEnd );
                range.Load( stream, DataFormats.Rtf );
            }
        }

        public void Store( FlowDocument content )
        {
            var file = GetFullPath( "General.rtf" );
            using( var stream = new FileStream( file, FileMode.Create, FileAccess.Write ) )
            {
                var range = new TextRange( content.ContentStart, content.ContentEnd );
                range.Save( stream, DataFormats.Rtf );
            }
        }

        public void Store( Stock stock, FlowDocument content )
        {
            var file = GetFullPath( stock.Company.Guid + ".rtf" );

            if( !File.Exists( file ) )
            {
                var range = new TextRange( content.ContentStart, content.ContentEnd );
                if( string.IsNullOrWhiteSpace( range.Text ) )
                {
                    // do not create empty files
                    return;
                }
            }

            using( var stream = new FileStream( file, FileMode.Create, FileAccess.Write ) )
            {
                var range = new TextRange( content.ContentStart, content.ContentEnd );
                range.Save( stream, DataFormats.Rtf );
            }
        }
    }
}
