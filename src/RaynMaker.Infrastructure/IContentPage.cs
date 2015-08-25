
using RaynMaker.Entities;
namespace RaynMaker.Infrastructure
{
    public interface IContentPage
    {
        void Initialize( Stock stock );
        void Cancel();
        void Complete();
    }
}
