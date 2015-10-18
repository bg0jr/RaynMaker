﻿using System;
using RaynMaker.Import.Spec;

namespace RaynMaker.Import.Web.ViewModels
{
    class FormatFactory
    {
        public static IFormat CreatePathSeriesFormat()
        {
            var format = new PathSeriesFormat( string.Empty );

            format.ValueFormat = new FormatColumn( "value", typeof( double ), "000,000.0000" );
            format.TimeAxisFormat = new FormatColumn( "time", typeof( int ), "0000" );

            return format;
        }

        internal static IFormat Create( Type type )
        {
            if( type == typeof( PathSeriesFormat ) )
            {
                return CreatePathSeriesFormat();
            }

            throw new NotSupportedException( "Unknown format type: " + type.Name );
        }
    }
}