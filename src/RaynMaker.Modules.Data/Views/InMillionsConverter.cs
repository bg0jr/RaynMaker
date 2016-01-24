using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using RaynMaker.Entities;

namespace RaynMaker.Data.Views
{
    class InMillionsConverter : Freezable, IValueConverter
    {
        private bool myInMillions;

        public bool InMillions
        {
            get { return myInMillions; }
            set
            {
                if( myInMillions == value )
                {
                    return;
                }

                myInMillions = value;

                foreach( var figure in Series )
                {
                    ( ( EntityBase )figure ).RaisePropertyChanged( () => figure.Value );
                }
            }
        }

        public static readonly DependencyProperty SeriesProperty = DependencyProperty.Register( "Series",
            typeof( IFigureSeries ), typeof( InMillionsConverter ), new PropertyMetadata( null ) );

        public IFigureSeries Series
        {
            get { return ( IFigureSeries )GetValue( SeriesProperty ); }
            set { SetValue( SeriesProperty, value ); }
        }

        public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
        {
            if( value == null )
            {
                return null;
            }

            return InMillions ? ( double )value / 1000000 : value;
        }

        public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
        {
            var text = ( string )value;

            if( string.IsNullOrEmpty( text ) )
            {
                return null;
            }

            var dValue = double.Parse( text, CultureInfo.InvariantCulture );
            return InMillions ? dValue * 1000000 : dValue;
        }

        protected override Freezable CreateInstanceCore()
        {
            return new InMillionsConverter();
        }
    }
}
