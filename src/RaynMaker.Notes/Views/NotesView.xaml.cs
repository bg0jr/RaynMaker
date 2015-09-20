using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using RaynMaker.Notes.ViewModels;

namespace RaynMaker.Notes.Views
{
    [Export]
    public partial class NotesView : UserControl
    {
        private NotesViewModel myViewModel;

        [ImportingConstructor]
        internal NotesView( NotesViewModel viewModel )
        {
            myViewModel = viewModel;

            InitializeComponent();

            cmbFontFamily.ItemsSource = Fonts.SystemFontFamilies.OrderBy( f => f.Source );
            cmbFontSize.ItemsSource = new List<double>() { 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72 };

            DataContext = myViewModel;

            Loaded += OnLoaded;
        }

        private void OnLoaded( object sender, RoutedEventArgs e )
        {
            myViewModel.Document = rtbEditor.Document;
        }

        // https://social.msdn.microsoft.com/Forums/vstudio/en-US/29340547-0fe6-42f9-8350-aa24ffc7d67e/richtextbox-insert-hyperlink-at-caret-position?forum=wpf
        // http://cxhelloworld.blogspot.de/2012/05/hyperlink-in-wpf-richtextbox.html

        private void rtbEditor_SelectionChanged( object sender, RoutedEventArgs e )
        {
            object temp = rtbEditor.Selection.GetPropertyValue( Inline.FontWeightProperty );
            btnBold.IsChecked = ( temp != DependencyProperty.UnsetValue ) && ( temp.Equals( FontWeights.Bold ) );
            temp = rtbEditor.Selection.GetPropertyValue( Inline.FontStyleProperty );
            btnItalic.IsChecked = ( temp != DependencyProperty.UnsetValue ) && ( temp.Equals( FontStyles.Italic ) );
            temp = rtbEditor.Selection.GetPropertyValue( Inline.TextDecorationsProperty );
            btnUnderline.IsChecked = ( temp != DependencyProperty.UnsetValue ) && ( temp.Equals( TextDecorations.Underline ) );

            temp = rtbEditor.Selection.GetPropertyValue( Inline.FontFamilyProperty );
            cmbFontFamily.SelectedItem = temp;
            temp = rtbEditor.Selection.GetPropertyValue( Inline.FontSizeProperty );
            cmbFontSize.Text = temp.ToString();
        }

        private void cmbFontFamily_SelectionChanged( object sender, SelectionChangedEventArgs e )
        {
            if( cmbFontFamily.SelectedItem != null )
                rtbEditor.Selection.ApplyPropertyValue( Inline.FontFamilyProperty, cmbFontFamily.SelectedItem );
        }

        private void cmbFontSize_TextChanged( object sender, TextChangedEventArgs e )
        {
            rtbEditor.Selection.ApplyPropertyValue( Inline.FontSizeProperty, cmbFontSize.Text );
        }

        private void rtbEditor_TextChanged( object sender, TextChangedEventArgs e )
        {
            Debug.WriteLine( "." );
        }
    }
}
