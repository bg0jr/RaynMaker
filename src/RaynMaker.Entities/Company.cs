using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using Plainion.Validation;
using RaynMaker.Entities.Datums;

namespace RaynMaker.Entities
{
    public class Company : EntityBase
    {
        private string myName;
        private string myHomepage;
        private string mySector;
        private string myCountry;

        public Company()
        {
            Guid = System.Guid.NewGuid().ToString();
            
            Stocks = new List<Stock>();
            References = new ObservableCollection<Reference>();

            CurrentAssets = new List<CurrentAssets>();
            Debts = new List<Debt>();
            Dividends = new List<Dividend>();
            EBITs = new List<EBIT>();
            Equities = new List<Equity>();
            InterestExpenses = new List<InterestExpense>();
            CurrentLiabilities = new List<CurrentLiabilities>();
            NetIncomes = new List<NetIncome>();
            Revenues = new List<Revenue>();
            SharesOutstandings = new List<SharesOutstanding>();
            Tags = new ObservableCollection<Tag>();
        }

        [Required]
        public string Guid { get; internal set; }
        
        [Required]
        public string Name
        {
            get { return myName; }
            set { SetProperty( ref myName, value ); }
        }

        [Url]
        public string Homepage
        {
            get { return myHomepage; }
            set { SetProperty( ref myHomepage, value ); }
        }

        public string Sector
        {
            get { return mySector; }
            set { SetProperty( ref mySector, value ); }
        }

        public string Country
        {
            get { return myCountry; }
            set { SetProperty( ref myCountry, value ); }
        }

        [ValidateObject]
        public virtual ObservableCollection<Reference> References { get; private set; }

        [ValidateObject]
        public virtual IList<Stock> Stocks { get; private set; }

        [ValidateObject]
        public virtual IList<CurrentAssets> CurrentAssets { get; private set; }

        [ValidateObject]
        public virtual IList<Debt> Debts { get; private set; }

        [ValidateObject]
        public virtual IList<Dividend> Dividends { get; private set; }

        [ValidateObject]
        public virtual IList<EBIT> EBITs { get; private set; }

        [ValidateObject]
        public virtual IList<Equity> Equities { get; private set; }

        [ValidateObject]
        public virtual IList<InterestExpense> InterestExpenses { get; private set; }

        [ValidateObject]
        public virtual IList<CurrentLiabilities> CurrentLiabilities { get; private set; }

        [ValidateObject]
        public virtual IList<NetIncome> NetIncomes { get; private set; }

        [ValidateObject]
        public virtual IList<Revenue> Revenues { get; private set; }

        [ValidateObject]
        public virtual IList<SharesOutstanding> SharesOutstandings { get; private set; }

        [ValidateObject]
        public virtual ObservableCollection<Tag> Tags { get; private set; }
    }
}
