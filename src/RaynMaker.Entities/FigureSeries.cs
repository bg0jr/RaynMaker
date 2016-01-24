using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using Plainion;
using Plainion.Validation;

namespace RaynMaker.Entities
{
    public class FigureSeries : IFigureSeries, ICollection<IFigure>
    {
        private bool myCurrencyIsFrozen;
        private IComparer<IFigure> myComparer;

        [Required, ValidateObject]
        private List<IFigure> myValues;

        public static IFigureSeries Empty = new FigureSeries();

        private FigureSeries()
        {
            Name = "Empty";
            myValues = new List<IFigure>();
        }

        private FigureSeries( Type figureType, string name )
        {
            Contract.RequiresNotNull( figureType, "figureType" );

            FigureType = figureType;
            Name = name;

            myValues = new List<IFigure>();
            EnableCurrencyCheck = true;
        }

        public FigureSeries( Type figureType )
            : this( figureType, "CollectionOf" + figureType.Name )
        {
        }

        public FigureSeries( Type figureType, params IFigure[] items )
            : this( figureType, ( IEnumerable<IFigure> )items )
        {
        }

        public FigureSeries( Type figureType, IEnumerable<IFigure> items )
            : this( figureType )
        {
            Contract.RequiresNotNull( items, "items" );

            foreach( var item in items )
            {
                Add( item );
            }
        }

        public Type FigureType { get; private set; }

        public string Name { get; private set; }

        public Currency Currency { get; private set; }

        /// <summary>
        /// If set to false "Currency" property will return null
        /// </summary>
        public bool EnableCurrencyCheck { get; set; }

        public void Add( IFigure figure )
        {
            Contract.Invariant( figure != null, "Empty series must not be modified" );

            Contract.RequiresNotNull( figure, "figure" );
            Contract.Requires( figure.GetType() == FigureType, "[{0}] FigureType mismatch: expected={1}, actual={2}", Name, FigureType, figure.GetType() );

            CheckCurrency( figure );

            if( myComparer == null )
            {
                myComparer = new FigureByPeriodComparer();
            }

            var index = myValues.BinarySearch( figure, myComparer );
            if( index < 0 ) index = ~index;
            myValues.Insert( index, figure );
        }

        private void CheckCurrency( IFigure figure )
        {
            if( !EnableCurrencyCheck )
            {
                return;
            }

            var currencyFigure = figure as ICurrencyFigure;

            if( !myCurrencyIsFrozen )
            {
                if( currencyFigure != null )
                {
                    Currency = currencyFigure.Currency;
                }

                myCurrencyIsFrozen = true;
            }

            if( currencyFigure != null )
            {
                // we cannot enforce collection currency != null because of DerivedFigure
                Contract.Requires( Currency == currencyFigure.Currency,
                    "[{0}] Currency inconsistencies found: expected={1}, actual={2}", Name, Currency, currencyFigure.Currency );
            }
            else
            {
                Contract.Requires( Currency == null, "[{0}] Currency inconsistencies found: expected={1}, actual={2}", Name, Currency, null );
            }
        }

        public bool Remove( IFigure figure )
        {
            Contract.RequiresNotNull( figure, "figure" );

            return myValues.Remove( figure );
        }

        public int Count
        {
            get { return myValues.Count; }
        }

        public IEnumerator<IFigure> GetEnumerator()
        {
            return myValues.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return myValues.GetEnumerator();
        }

        public void Clear()
        {
            myValues.Clear();
        }

        public bool Contains( IFigure item )
        {
            return myValues.Contains( item );
        }

        public void CopyTo( IFigure[] array, int arrayIndex )
        {
            myValues.CopyTo( array, arrayIndex );
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public void VerifyCurrencyConsistency()
        {
            // do not check for EnableCurrencyCheck - verification is explicitly called
            var currencies = myValues
                .Select( GetCurrency )
                .Distinct()
                .ToList();

            Contract.Invariant( currencies.Count <= 1, "[{0}] Currency inconsistencies found: More than one currency used '{1}'",
                Name, string.Join( ",", currencies ) );
        }

        private string GetCurrency( IFigure figure )
        {
            var currencyFigure = figure as ICurrencyFigure;
            if( currencyFigure == null || currencyFigure.Currency == null )
            {
                return "None";
            }

            return currencyFigure.Currency.Name;
        }
    }
}
