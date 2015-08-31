using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace RaynMaker.Import.Web.Views
{
    // http://stackoverflow.com/questions/2245928/mvvm-and-the-textboxs-selectedtext-property
    class TextBoxBinding
    {
        public static string GetSelectedText( DependencyObject obj )
        {
            return ( string )obj.GetValue( SelectedTextProperty );
        }

        public static void SetSelectedText( DependencyObject obj, string value )
        {
            obj.SetValue( SelectedTextProperty, value );
        }

        public static readonly DependencyProperty SelectedTextProperty = DependencyProperty.RegisterAttached( "SelectedText",
                typeof( string ), typeof( TextBoxBinding ),
                new FrameworkPropertyMetadata( null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedTextChanged ) );

        private static void OnSelectedTextChanged( DependencyObject obj, DependencyPropertyChangedEventArgs e )
        {
            var textBox = ( TextBox )obj;

            if( e.OldValue != null && e.NewValue != null )
            {
                textBox.SelectionChanged += OnSelectionChanged;
            }
            else if( e.OldValue != null && e.NewValue == null )
            {
                textBox.SelectionChanged -= OnSelectionChanged;
            }

            string newValue = e.NewValue as string;

            if( newValue != null && newValue != textBox.SelectedText )
            {
                textBox.SelectedText = newValue as string;
            }
        }

        private static void OnSelectionChanged( object sender, RoutedEventArgs e )
        {
            var textBox = ( TextBox )sender;
            SetSelectedText( textBox, textBox.SelectedText );
        }
    }
}
