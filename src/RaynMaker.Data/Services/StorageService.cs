using System.ComponentModel.Composition;
using System.IO;
using System.Windows;
using System.Windows.Documents;
using RaynMaker.Entities;
using RaynMaker.Infrastructure;

namespace RaynMaker.Data.Services
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

        public void Load( Stock stock, FlowDocument target )
        {
            var file = Path.Combine( myProjectHost.Project.StorageRoot, "Notes." + stock.Isin + ".rtf" );
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

        public void Store( Stock stock, FlowDocument content )
        {
            var file = Path.Combine( myProjectHost.Project.StorageRoot, "Notes." + stock.Isin + ".rtf" );
            using( var stream = new FileStream( file, FileMode.Create, FileAccess.Write ) )
            {
                var range = new TextRange( content.ContentStart, content.ContentEnd );
                range.Save( stream, DataFormats.Rtf );
            }
        }
    }
}
