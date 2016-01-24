using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure.Pluralization;
using Plainion;
using RaynMaker.Entities.Figures;

namespace RaynMaker.Entities
{
    public class Dynamics
    {
        public static readonly IEnumerable<Type> AllFigures = new Type[] { 
            typeof( SharesOutstanding ), 
            typeof( NetIncome ),                
            typeof( Equity ),
            typeof( Dividend ),                 
            typeof( CurrentAssets ),                
            typeof( CurrentLiabilities ),                
            typeof( TotalLiabilities ),
            typeof( Revenue ),                
            typeof( EBIT ),                
            typeof( InterestExpense ),
            typeof( Price )
            };

        public static IList<T> GetRelationship<T>( Stock stock ) where T : IFigure
        {
            return ( IList<T> )GetRelationship( stock, typeof( T ) );
        }

        public static IEnumerable<IFigure> GetRelationship( Stock stock, Type figureType )
        {
            var service = new EnglishPluralizationService();
            var collectionName = service.Pluralize( figureType.Name );

            var property = typeof( Company ).GetProperty( collectionName );
            if( property != null )
            {
                return ( IEnumerable<IFigure> )property.GetValue( stock.Company );
            }

            property = typeof( Stock ).GetProperty( collectionName );
            if( property != null )
            {
                return ( IEnumerable<IFigure> )property.GetValue( stock );
            }

            throw new ArgumentException( string.Format( "No relationship (navigation property) found with name {0} on Company or Stock", collectionName ) );
        }

        public static IFigureSeries GetSeries( Stock stock, Type figureType, bool enableCurrencyCheck )
        {
            var series= new FigureSeries( figureType );
            series.EnableCurrencyCheck = enableCurrencyCheck;

            foreach( var item in GetRelationship( stock, figureType ) )
            {
                series.Add(item);
            }

            return series;
        }

        public static AbstractFigure CreateFigure( Stock stock, Type figureType, IPeriod period, Currency currency )
        {
            var figure = ( AbstractFigure )Activator.CreateInstance( figureType );
            figure.Period = period;

            var currencyFigure = figure as AbstractCurrencyFigure;
            if( currencyFigure != null )
            {
                currencyFigure.Currency = currency;
            }

            var foreignKey = figureType.GetProperty( "Stock" );
            if( foreignKey != null )
            {
                foreignKey.SetValue( figure, stock );
            }
            else
            {
                foreignKey = figureType.GetProperty( "Company" );

                Contract.Invariant( foreignKey != null, "ForeignKey detection failed" );

                foreignKey.SetValue( figure, stock.Company );
            }

            return figure;
        }
    }
}
