using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaynMaker.Modules.Import.Spec.v2
{
    [AttributeUsage( AttributeTargets.Property )]
    sealed class CollectionNotEmptyAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid( object value, ValidationContext validationContext )
        {
            if( IsEmpty( ( IEnumerable )value ) )
            {
                return new ValidationResult( validationContext.MemberName + " must not be empty", new List<string> { validationContext.MemberName } );
            }

            return ValidationResult.Success;
        }

        private bool IsEmpty( IEnumerable source )
        {
            return !source.GetEnumerator().MoveNext();
        }
    }
}
