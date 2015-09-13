using System;
using System.Data;
using RaynMaker.Import.Spec;

namespace RaynMaker.Import.Documents
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
