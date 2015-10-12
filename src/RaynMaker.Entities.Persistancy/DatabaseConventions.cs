using System;
using System.Data.Entity.Infrastructure.Pluralization;

namespace RaynMaker.Entities.Persistancy
{
    class DatabaseConventions
    {
        public static string GetTableName( Type datumType )
        {
            var service = new EnglishPluralizationService();
            return service.Pluralize( datumType.Name );
        }
    }
}
