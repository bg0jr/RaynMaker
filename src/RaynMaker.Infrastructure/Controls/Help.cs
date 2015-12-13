using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using RaynMaker.Infrastructure.Services;

namespace RaynMaker.Infrastructure.Controls
{
    // Inspired by: https://peteohanlon.wordpress.com/2009/05/01/easy-help-with-wpf/
    public static class Help
    {
        static Help()
        {
            // Rather than having to manually associate the Help command, let's take care of this here.
            CommandManager.RegisterClassCommandBinding( typeof( FrameworkElement ), new CommandBinding( ApplicationCommands.Help,
                new ExecutedRoutedEventHandler( Executed ),
                new CanExecuteRoutedEventHandler( CanExecute ) ) );
        }

        private static void CanExecute( object sender, CanExecuteRoutedEventArgs args )
        {
            var el = sender as FrameworkElement;
            if( el == null )
            {
                return;
            }
            
            string topic = FindTopic( el );

            if( !string.IsNullOrEmpty( topic ) )
            {
                args.CanExecute = true;
            }
        }

        private static void Executed( object sender, ExecutedRoutedEventArgs args )
        {
            var parent = args.OriginalSource as DependencyObject;

            HelpService.ShowHelp( FindTopic( parent ), GetKeyword( parent ) );
        }

        private static string FindTopic( DependencyObject sender )
        {
            if( sender == null )
            {
                return null;
            }

            string topic = GetTopic( sender );

            return !string.IsNullOrEmpty( topic ) ? topic : FindTopic( VisualTreeHelper.GetParent( sender ) );
        }

        public static readonly DependencyProperty TopicProperty = DependencyProperty.RegisterAttached( "Topic", typeof( string ), typeof( Help ) );

        public static string GetTopic( DependencyObject d )
        {
            return ( string )d.GetValue( TopicProperty );
        }

        public static void SetTopic( DependencyObject d, string value )
        {
            d.SetValue( TopicProperty, value );
        }

        public static readonly DependencyProperty KeywordProperty = DependencyProperty.RegisterAttached( "Keyword", typeof( string ), typeof( Help ),
            new PropertyMetadata( OnKeywordChanged ) );

        private static void OnKeywordChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
        {
            var frameworkElement = d as FrameworkElement;
            if( frameworkElement == null )
            {
                return;
            }

            //frameworkElement.ToolTip = new TextBlock( new Run( "Press F1 for help" ) )
            //{
            //    Margin = new Thickness( 5 )
            //};
        }

        public static string GetKeyword( DependencyObject d )
        {
            return ( string )d.GetValue( KeywordProperty );
        }

        public static void SetKeyword( DependencyObject d, string value )
        {
            d.SetValue( KeywordProperty, value );
        }
    }
}
