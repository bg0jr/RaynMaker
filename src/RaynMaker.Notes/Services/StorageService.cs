using System.ComponentModel.Composition;
using System.IO;
using System.Windows;
using System.Windows.Documents;
using RaynMaker.Entities;
using RaynMaker.Infrastructure;

namespace RaynMaker.Notes
{
    [Export]
    class StorageService
    {
        private IProjectHost myProjectHost;

        [ImportingConstructor]
        public StorageService( IProjectHost projectHost )
        {
            myProjectHost = projectHost;
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
            return Path.Combine( myProjectHost.Project.StorageRoot, "Notes", fileName );
        }

        public void Load( Stock stock, FlowDocument target )
        {
            var file = GetFullPath( stock.Isin + ".rtf" );
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
            var file = GetFullPath( stock.Isin + ".rtf" );

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
