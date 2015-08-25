using System;
using System.Collections.Generic;

namespace RaynMaker.Entities.Datums
{
    public class KnownDatums
    {
        public static readonly IEnumerable<Type> AllExceptPrice = new Type[] { 
            typeof( SharesOutstanding ), 
            typeof( NetIncome ),                
            typeof( Equity ) ,
            typeof( Dividend ),                 
            typeof( Assets ) ,                
            typeof( Liabilities ) ,                
            typeof( Debt ) ,
            typeof( Revenue ) ,                
            typeof( EBIT ) ,                
            typeof( InterestExpense ) 
            };

    }
}
