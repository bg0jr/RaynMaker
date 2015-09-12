using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure.Pluralization;
using Plainion;
using RaynMaker.Entities.Datums;

namespace RaynMaker.Entities
{
    public class Dynamics
    {
        public static readonly IEnumerable<Type> AllDatums = new Type[] { 
            typeof( SharesOutstanding ), 
            typeof( NetIncome ),                
            typeof( Equity ) ,
            typeof( Dividend ),                 
            typeof( Assets ) ,                
            typeof( Liabilities ) ,                
            typeof( Debt ) ,
            typeof( Revenue ) ,                
            typeof( EBIT ) ,                
            typeof( InterestExpense ) ,
            typeof(Price)
            };

        public static IList<T> GetRelationship<T>( Stock stock ) where T : IDatum
        {
            return ( IList<T> )GetRelationship( stock, typeof( T ) );
        }

        public static IEnumerable<IDatum> GetRelationship( Stock stock, Type datumType )
        {
            var service = new EnglishPluralizationService();
            var collectionName = service.Pluralize( datumType.Name );

            var property = typeof( Company ).GetProperty( collectionName );
            if( property != null )
            {
                return ( IEnumerable<IDatum> )property.GetValue( stock.Company );
            }

            property = typeof( Stock ).GetProperty( collectionName );
            if( property != null )
            {
                return ( IEnumerable<IDatum> )property.GetValue( stock );
            }

            throw new ArgumentException( string.Format( "No relationship (navigation property) found with name {0} on Company or Stock", collectionName ) );
        }

        public static IDatumSeries GetDatumSeries( Stock stock, Type datumType )
        {
            return new DatumSeries( datumType, GetRelationship( stock, datumType ) );
        }

        public static AbstractDatum CreateDatum( Stock stock, Type datumType, IPeriod period, Currency currency )
        {
            var datum = ( AbstractDatum )Activator.CreateInstance( datumType );
            datum.Period = period;

            var currencyDatum = datum as AbstractCurrencyDatum;
            if( currencyDatum != null )
            {
                currencyDatum.Currency = currency;
            }

            var foreignKey = datumType.GetProperty( "Stock" );
            if( foreignKey != null )
            {
                foreignKey.SetValue( datum, stock );
            }
            else
            {
                foreignKey = datumType.GetProperty( "Company" );

                Contract.Invariant( foreignKey != null, "ForeignKey detection failed" );

                foreignKey.SetValue( datum, stock.Company );
            }

            return datum;
        }
    }
}
