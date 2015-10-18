using System;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Plainion.Prism.Interactivity.InteractionRequest;

namespace RaynMaker.Infrastructure.Controls
{
    public partial class DefaultNotificationView : UserControl, IInteractionRequestAware
    {
        public DefaultNotificationView()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty ViewContentProperty = DependencyProperty.Register( "ViewContent",
            typeof( object ), typeof( DefaultNotificationView ), new PropertyMetadata( null ) );

        public object ViewContent
        {
            get { return ( object )GetValue( DefaultNotificationView.ViewContentProperty ); }
            set { SetValue( DefaultNotificationView.ViewContentProperty, value ); }
        }

        public Action FinishInteraction { get; set; }

        public INotification Notification
        {
            get { return DataContext as INotification; }
            set
            {
                DataContext = value;

                if( value != null && value.Content != null )
                {
                    ViewContent = value.Content;
                }
            }
        }

        private void OnOk( object sender, RoutedEventArgs e )
        {
            FinishInteraction();
        }
    }
}
