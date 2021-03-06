﻿using System.Collections.Generic;
using System.Linq;
using Plainion;
using RaynMaker.Entities;

namespace RaynMaker.Modules.Analysis.Engine
{
    class MissingDataForPeriod : IFigureProviderFailure
    {
        public MissingDataForPeriod( string figure, object defaultValue, IPeriod period, params IPeriod[] periods )
        {
            Contract.RequiresNotNullNotEmpty( figure, "figure" );
            Contract.RequiresNotNull( period, "period" );

            Figure = figure;
            DefaultValue = defaultValue;

            var allPeriods = periods.ToList();
            allPeriods.Add( period );

            Period = allPeriods;
        }

        public string Figure { get; private set; }

        public object DefaultValue { get; private set; }

        public IReadOnlyCollection<IPeriod> Period { get; private set; }

        public override string ToString()
        {
            return string.Format( "No data found for '{0}' in periods: {1}", Figure, string.Join( ",", Period ) );
        }
    }
}
