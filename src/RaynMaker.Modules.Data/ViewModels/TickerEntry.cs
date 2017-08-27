using System;
using System.Linq;
using Prism.Mvvm;
using RaynMaker.Entities;
using RaynMaker.Entities.Figures;

namespace RaynMaker.Data.ViewModels
{
    class TickerEntry : BindableBase
    {
        private DateTime myToday;
        private Price myPreviousPrice;
        private Price myCurrentPrice;

        public TickerEntry(Stock stock)
        {
            Stock = stock;

            InitToday();

            InitPrices();
        }

        public Stock Stock { get; private set; }

        private void InitToday()
        {
            myToday = DateTime.Today;

            if (myToday.DayOfWeek == DayOfWeek.Saturday)
            {
                myToday = myToday.Subtract(TimeSpan.FromDays(1));
            }
            else if (myToday.DayOfWeek == DayOfWeek.Sunday)
            {
                myToday = myToday.Subtract(TimeSpan.FromDays(2));
            }
        }

        private void InitPrices()
        {
            PreviousPrice = Stock.Prices
                .OrderByDescending(p => p.Period)
                .FirstOrDefault();
            if (PreviousPrice == null)
            {
                return;
            }

            // take ">=" because we "move" today to friday in case today is actally weekend
            if (((DayPeriod)PreviousPrice.Period).Day >= myToday)
            {
                CurrentPrice = PreviousPrice;

                PreviousPrice = Stock.Prices
                    .OrderByDescending(p => p.Period)
                    .Skip(1)
                    .FirstOrDefault();
            }
        }

        public string Company { get { return Stock.Company.Name; } }

        public string Isin { get { return Stock.Isin; } }

        public Price PreviousPrice
        {
            get { return myPreviousPrice; }
            set
            {
                if (SetProperty(ref myPreviousPrice, value))
                {
                    RaisePropertyChanged(nameof(PreviousPriceDate));
                    RaisePropertyChanged(nameof(PreviousPriceValue));
                    RaisePropertyChanged(nameof(Change));
                }
            }
        }

        public string PreviousPriceDate { get { return PreviousPrice != null ? ((DayPeriod)PreviousPrice.Period).Day.ToShortDateString() : null; } }

        public string PreviousPriceValue { get { return PreviousPrice != null ? PreviousPrice.Value.Value + " " + PreviousPrice.Currency.Symbol : null; } }

        public Price CurrentPrice
        {
            get { return myCurrentPrice; }
            set
            {
                if (SetProperty(ref myCurrentPrice, value))
                {
                    RaisePropertyChanged(nameof(CurrentPriceDate));
                    RaisePropertyChanged(nameof(CurrentPriceValue));
                    RaisePropertyChanged(nameof(Change));
                }
            }
        }

        public string CurrentPriceDate { get { return CurrentPrice != null ? ((DayPeriod)CurrentPrice.Period).Day.ToShortDateString() : null; } }

        public string CurrentPriceValue { get { return CurrentPrice != null ? CurrentPrice.Value.Value + " " + CurrentPrice.Currency.Symbol : null; } }

        public double? Change
        {
            get
            {
                if (PreviousPrice != null && CurrentPrice != null && PreviousPrice.Currency == CurrentPrice.Currency)
                {
                    return (CurrentPrice.Value.Value - PreviousPrice.Value.Value) / PreviousPrice.Value.Value * 100;
                }

                return null;
            }
        }
    }
}
