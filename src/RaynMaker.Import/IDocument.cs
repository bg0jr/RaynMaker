using System.Data;
using RaynMaker.Import.Spec;

namespace RaynMaker.Import
{
    public interface IDocument
    {
        string Location { get; }
        DataTable ExtractTable( IFormat format );
    }
}
