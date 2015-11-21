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
    public class DatumSeries : IDatumSeries, ICollection<IDatum>
    {
        private bool myCurrencyIsFrozen;
        private IComparer<IDatum> myComparer;

        [Required, ValidateObject]
        private List<IDatum> myValues;

        public static IDatumSeries Empty = new DatumSeries();

        private DatumSeries()
        {
            Name = "Empty";
            myValues = new List<IDatum>();
        }

        private DatumSeries( Type datumType, string name )
        {
            Contract.RequiresNotNull( datumType, "datumType" );

            DatumType = datumType;
            Name = name;

            myValues = new List<IDatum>();
            EnableCurrencyCheck = true;
        }

        public DatumSeries( Type datumType )
            : this( datumType, "CollectionOf" + datumType.Name )
        {
        }

        public DatumSeries( Type datumType, params IDatum[] items )
            : this( datumType, ( IEnumerable<IDatum> )items )
        {
        }

        public DatumSeries( Type datumType, IEnumerable<IDatum> items )
            : this( datumType )
        {
            Contract.RequiresNotNull( items, "items" );

            foreach( var item in items )
            {
                Add( item );
            }
        }

        public Type DatumType { get; private set; }

        public string Name { get; private set; }

        public Currency Currency { get; private set; }

        /// <summary>
        /// If set to false "Currency" property will return null
        /// </summary>
        public bool EnableCurrencyCheck { get; set; }

        public void Add( IDatum datum )
        {
            Contract.Invariant( datum != null, "Empty series must not be modified" );

            Contract.RequiresNotNull( datum, "datum" );
            Contract.Requires( datum.GetType() == DatumType, "[{0}] DatumType mismatch: expected={1}, actual={2}", Name, DatumType, datum.GetType() );

            CheckCurrency( datum );

            if( myComparer == null )
            {
                myComparer = new DatumByPeriodComparer();
            }

            var index = myValues.BinarySearch( datum, myComparer );
            if( index < 0 ) index = ~index;
            myValues.Insert( index, datum );
        }

        private void CheckCurrency( IDatum datum )
        {
            if( !EnableCurrencyCheck )
            {
                return;
            }

            var currencyDatum = datum as ICurrencyDatum;

            if( !myCurrencyIsFrozen )
            {
                if( currencyDatum != null )
                {
                    Currency = currencyDatum.Currency;
                }

                myCurrencyIsFrozen = true;
            }

            if( currencyDatum != null )
            {
                // we cannot enforce collection currency != null because of DerivedDatum
                Contract.Requires( Currency == currencyDatum.Currency,
                    "[{0}] Currency inconsistencies found: expected={1}, actual={2}", Name, Currency, currencyDatum.Currency );
            }
            else
            {
                Contract.Requires( Currency == null, "[{0}] Currency inconsistencies found: expected={1}, actual={2}", Name, Currency, null );
            }
        }

        public bool Remove( IDatum datum )
        {
            Contract.RequiresNotNull( datum, "datum" );

            return myValues.Remove( datum );
        }

        public int Count
        {
            get { return myValues.Count; }
        }

        public IEnumerator<IDatum> GetEnumerator()
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

        public bool Contains( IDatum item )
        {
            return myValues.Contains( item );
        }

        public void CopyTo( IDatum[] array, int arrayIndex )
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

        private string GetCurrency( IDatum datum )
        {
            var currencyDatum = datum as ICurrencyDatum;
            if( currencyDatum == null || currencyDatum.Currency == null )
            {
                return "None";
            }

            return currencyDatum.Currency.Name;
        }
    }
}
