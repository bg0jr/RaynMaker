using System;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using Microsoft.Practices.Prism.Mvvm;

namespace RaynMaker.Entities
{
    public abstract class EntityBase : BindableBase
    {
        [Required]
        public long Id { get; set; }
        
        public void RaisePropertyChanged( string propertyName )
        {
            OnPropertyChanged( propertyName );
        }

        public void RaisePropertyChanged<T>( Expression<Func<T>> propertyExpression )
        {
            OnPropertyChanged( propertyExpression );
        }
    }
}
