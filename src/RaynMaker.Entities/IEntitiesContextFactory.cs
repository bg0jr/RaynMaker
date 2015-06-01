
namespace RaynMaker.Entities
{
    public interface IEntitiesContextFactory
    {
        IEntityContext Create( string path );
    }
}
