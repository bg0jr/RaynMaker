using Maui;

namespace RaynMaker.Import
{
    public interface IDatumProvider
    {
        string Datum { get; }
        SingleResultValue<T> FetchSingle<T>();
        IResultPolicy Fetch();
    }
}
