using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RaynMaker.Modules.Import.Spec.v2
{
    class DocumentTypeNotNoneAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid( object value, ValidationContext validationContext )
        {
            if( ( ( DocumentType )value ) == DocumentType.None )
            {
                return new ValidationResult( validationContext.MemberName + " must not be DocumentType.None", new List<string> { validationContext.MemberName } );
            }

            return ValidationResult.Success;
        }
    }
}
