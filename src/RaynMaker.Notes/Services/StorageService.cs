using System.ComponentModel.Composition;
using System.IO;
using System.Windows;
using System.Windows.Documents;
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

        public void Load( TextRange target )
        {
            var file = Path.Combine( myProjectHost.Project.StorageRoot, "Notes.rtf" );
            if( !File.Exists( file ) )
            {
                return;
            }

            using( var stream = new FileStream( file, FileMode.Open, FileAccess.Read ) )
            {
                target.Load( stream, DataFormats.Rtf );
            }
        }

        public void Store( TextRange content )
        {
            var file = Path.Combine( myProjectHost.Project.StorageRoot, "Notes.rtf" );
            using( var stream = new FileStream( file, FileMode.Create, FileAccess.Write ) )
            {
                content.Save( stream, DataFormats.Rtf );
            }
        }

    }
}
