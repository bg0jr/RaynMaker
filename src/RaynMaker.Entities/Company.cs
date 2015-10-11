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

            Assets = new List<Assets>();
            Debts = new List<Debt>();
            Dividends = new List<Dividend>();
            EBITs = new List<EBIT>();
            Equities = new List<Equity>();
            InterestExpenses = new List<InterestExpense>();
            Liabilities = new List<Liabilities>();
            NetIncomes = new List<NetIncome>();
            Revenues = new List<Revenue>();
            SharesOutstandings = new List<SharesOutstanding>();
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
        public virtual IList<Assets> Assets { get; private set; }

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
        public virtual IList<Liabilities> Liabilities { get; private set; }

        [ValidateObject]
        public virtual IList<NetIncome> NetIncomes { get; private set; }

        [ValidateObject]
        public virtual IList<Revenue> Revenues { get; private set; }

        [ValidateObject]
        public virtual IList<SharesOutstanding> SharesOutstandings { get; private set; }
    }
}
