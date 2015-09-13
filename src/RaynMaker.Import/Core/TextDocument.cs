using System;
using System.Data;
using RaynMaker.Import.Spec;

namespace RaynMaker.Import.Core
{
    internal class TextDocument : IDocument
    {
        internal TextDocument( string file )
        {
            Location = file;
        }

        public string Location { get; private set; }
    }
}
