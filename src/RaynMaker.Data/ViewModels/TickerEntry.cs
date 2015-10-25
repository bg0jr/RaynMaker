using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RaynMaker.Entities;
using RaynMaker.Entities.Datums;

namespace RaynMaker.Data.ViewModels
{
    class TickerEntry
    {
        private Stock myStock;
        private DateTime myToday;

        public TickerEntry( Stock stock )
        {
            myStock = stock;

            InitToday();

            InitPrices();
        }

        private void InitPrices()
        {
            PreviousPrice = myStock.Prices
                .OrderByDescending( p => p.Period )
                .FirstOrDefault();
            if( PreviousPrice == null )
            {
                return;
            }

            if( ( ( DayPeriod )PreviousPrice.Period ).Day == myToday )
            {
                CurrentPrice = PreviousPrice;

                PreviousPrice = myStock.Prices
                    .OrderByDescending( p => p.Period )
                    .Skip( 1 )
                    .FirstOrDefault();
            }

            if( PreviousPrice != null )
            {
                PreviousPriceDate = ( ( DayPeriod )PreviousPrice.Period ).Day.ToShortDateString();
                PreviousPriceValue = PreviousPrice.Value.Value + " " + PreviousPrice.Currency.Symbol;
            }

            if( CurrentPrice != null )
            {
                CurrentPriceDate = ( ( DayPeriod )CurrentPrice.Period ).Day.ToShortDateString();
                CurrentPriceValue = CurrentPrice.Value.Value + " " + CurrentPrice.Currency.Symbol;
            }
        }

        private void InitToday()
        {
            myToday = DateTime.Today;

            if( myToday.DayOfWeek == DayOfWeek.Saturday )
            {
                myToday = myToday.Subtract( TimeSpan.FromDays( 1 ) );
            }
            else if( myToday.DayOfWeek == DayOfWeek.Sunday )
            {
                myToday = myToday.Subtract( TimeSpan.FromDays( 2 ) );
            }
        }

        public string Company { get { return myStock.Company.Name; } }

        public string Isin { get { return myStock.Isin; } }

        public Price PreviousPrice { get; private set; }

        public string PreviousPriceDate { get; private set; }

        public string PreviousPriceValue { get; private set; }

        public Price CurrentPrice { get; private set; }

        public string CurrentPriceDate { get; private set; }

        public string CurrentPriceValue { get; private set; }

        public string Change
        {
            get
            {
                if( PreviousPrice != null && CurrentPrice != null )
                {
                    return string.Format( "{0:0.00} %", ( CurrentPrice.Value.Value - PreviousPrice.Value.Value ) * 100 );
                }

                return null;
            }
        }
    }
}
