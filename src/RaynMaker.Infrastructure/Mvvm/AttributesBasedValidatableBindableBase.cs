using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using Plainion;
using Plainion.Prism.Mvvm;

namespace RaynMaker.Infrastructure.Mvvm
{
    /// <summary>
    /// Mvvm base class with binding and validation support using DataAnnotations.
    /// </summary>
    /// <remarks>
    /// http://blog.pluralsight.com/async-validation-wpf-prism
    /// </remarks>
    public class AttributesBasedValidatableBindableBase : ValidatableBindableBase, IValidationAware
    {
        public void SetErrors<TProperty>( Expression<Func<TProperty>> propertyExpression, IEnumerable<string> messages )
        {
            ErrorsContainer.SetErrors( propertyExpression, messages
                .Select( m => new ValidationFailure( Severity.Error, m ) ) );
        }

        public void SetErrors( string propertyName, IEnumerable<string> messages )
        {
            ErrorsContainer.SetErrors( propertyName, messages
                .Select( m => new ValidationFailure( Severity.Error, m ) ) );
        }

        protected override bool SetProperty<T>( ref T storage, T value, [CallerMemberName] string propertyName = null )
        {
            var result = base.SetProperty( ref storage, value, propertyName );

            if( result && !string.IsNullOrEmpty( propertyName ) )
            {
                ValidateProperty( propertyName );
            }
            return result;
        }

        public bool ValidateProperty( string propertyName )
        {
            Contract.RequiresNotNullNotEmpty( propertyName, "propertyName" );

            var propertyInfo = GetType().GetRuntimeProperty( propertyName );

            Contract.Requires( propertyInfo != null, "Invalid property name {0}", propertyName );

            var errors = new List<string>();
            
            bool isValid = TryValidateProperty( propertyInfo, errors );

            SetErrors( propertyInfo.Name, errors );

            return isValid;
        }

        public bool ValidateProperties()
        {
            var propertiesWithChangedErrors = new List<string>();

            var propertiesToValidate = GetType().GetRuntimeProperties()
                .Where( c => c.GetCustomAttributes( typeof( ValidationAttribute ) ).Any() );

            foreach( var propertyInfo in propertiesToValidate )
            {
                var propertyErrors = new List<string>();

                TryValidateProperty( propertyInfo, propertyErrors );

                SetErrors( propertyInfo.Name, propertyErrors );
            }

            return ErrorsContainer.HasErrors;
        }

        private bool TryValidateProperty( PropertyInfo propertyInfo, List<string> propertyErrors )
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext( this ) { MemberName = propertyInfo.Name };
            var propertyValue = propertyInfo.GetValue( this );

            bool isValid = Validator.TryValidateProperty( propertyValue, context, results );

            if( results.Any() )
            {
                propertyErrors.AddRange( results.Select( c => c.ErrorMessage ) );
            }

            return isValid;
        }
    }
}
