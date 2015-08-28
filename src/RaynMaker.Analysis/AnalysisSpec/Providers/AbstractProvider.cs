﻿using System.Collections.Generic;
using Plainion;
using RaynMaker.Analysis.Engine;
using RaynMaker.Entities;

namespace RaynMaker.Analysis.AnalysisSpec.Providers
{
    public abstract class AbstractProvider : IFigureProvider
    {
        protected AbstractProvider( string name )
        {
            Contract.RequiresNotNullNotEmpty( name, "name" );

            Name = name;

            PreserveCurrency = true;
        }

        public string Name { get; private set; }

        /// <summary>
        /// Indicates that result should take over currency of input.
        /// </summary>
        public bool PreserveCurrency { get; set; }

        public abstract object ProvideValue( IFigureProviderContext context );

        protected void EnsureCurrencyConsistancy( IDatumSeries lhs, IDatumSeries rhs )
        {
            Contract.Requires( lhs.Currency == null || rhs.Currency == null || lhs.Currency == rhs.Currency,
                "Currency inconsistencies detected: {0}.Currency={1} vs {2}.Currency={3}",
                lhs.Name, lhs.Currency, rhs.Name, rhs.Currency );
        }
    }
}