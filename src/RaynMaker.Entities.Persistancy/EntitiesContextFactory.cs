using System.ComponentModel.Composition;

namespace RaynMaker.Entities.Persistancy
{
    [Export]
    class EntitiesContextFactory : IEntitiesContextFactory
    {
        public IEntityContext Create()
        {
            return new EntitiesContext();
        }
    }
}
