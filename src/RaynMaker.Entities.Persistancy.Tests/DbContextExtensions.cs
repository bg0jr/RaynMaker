using System;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Text;

namespace RaynMaker.Entities.Persistancy.Tests
{
    static class DbContextExtensions
    {
        public static void SaveChangesSafe( this DbContext self )
        {
            try
            {
                self.SaveChanges();
            }
            catch( DbEntityValidationException ex )
            {
                var sb = new StringBuilder( "Entity validation failed for: " );
                sb.AppendLine();

                foreach( var result in ex.EntityValidationErrors )
                {
                    foreach( var error in result.ValidationErrors )
                    {
                        sb.AppendLine( result.Entry.Entity.GetType().Name + "." + error.PropertyName + ": " + error.ErrorMessage );
                    }
                }

                throw new InvalidOperationException( sb.ToString(), ex );
            }
        }
    }
}
