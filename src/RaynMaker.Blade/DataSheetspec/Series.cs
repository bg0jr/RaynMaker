using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Windows.Markup;
using Plainion;
using Plainion.Validation;
using RaynMaker.Blade.Entities;

namespace RaynMaker.Blade.DataSheetSpec
{
    [DefaultProperty( "Values" ), ContentProperty( "Values" )]
    public class Series : IDatumSeries
    {
        private List<IDatum> myValues;

        public static Series Empty = CreateEmpty();

        private static Series CreateEmpty()
        {
            var series = new Series();
            series.Freeze();
            return series;
        }

        public Series()
        {
            myValues = new List<IDatum>();
        }

        internal Series( params IDatum[] items )
        {
            myValues = new List<IDatum>( items );
        }

        [Required, ValidateObject]
        public List<IDatum> Values
        {
            get { return IsFrozen ? null : myValues; }
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

        public string Name { get; private set; }

        public Type DatumType { get; private set; }

        public Currency Currency { get; private set; }

        public bool IsFrozen { get; private set; }

        public void Freeze()
        {
            if( IsFrozen )
            {
                return;
            }

            try
            {
                if( myValues.Count == 0 )
                {
                    return;
                }

                var valueTypes = myValues
                    .Select( v => v.GetType() )
                    .Distinct()
                    .ToList();

                Contract.Invariant( valueTypes.Count == 1, "Found multiple datum types in one series: {0}", string.Join( ",", valueTypes ) );

                DatumType = valueTypes.Single();
                Name = "CollectionOf" + DatumType.Name;

                var currencyDatums = myValues
                    .OfType<ICurrencyDatum>()
                    .ToList();

                if( currencyDatums.Any() )
                {
                    Contract.Invariant( myValues.Count == currencyDatums.Count, "{0}: Not all values have a currency", Name );

                    var currencies = currencyDatums
                        .Select( v => v.Currency )
                        .Distinct()
                        .ToList();

                    Contract.Requires( currencies.Count == 1, "{0}: Currency inconsistencies found: {1}", Name, string.Join( ",", currencies ) );

                    Currency = currencies.Single();
                }
            }
            finally
            {
                IsFrozen = true;
            }
        }

        public void Unfreeze()
        {
            IsFrozen = false;
        }
    }
}
