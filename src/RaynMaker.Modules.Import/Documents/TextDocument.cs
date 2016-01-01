using System;
using System.Data;
using RaynMaker.Modules.Import.Spec;

namespace RaynMaker.Modules.Import.Documents
{
    public class TextDocument : IDocument
    {
        internal TextDocument( string file )
        {
            Location = new Uri( file );
        }

        public Uri Location { get; private set; }
    }
}
