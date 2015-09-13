using System.Data;

namespace RaynMaker.Import
{
    public interface IDocumentParser
    {
        DataTable ExtractTable();
    }
}
