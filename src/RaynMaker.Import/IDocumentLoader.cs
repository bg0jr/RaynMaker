using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RaynMaker.Import
{
    public interface IDocumentLoader
    {
        IDocument Load( Uri uri );
    }
}
