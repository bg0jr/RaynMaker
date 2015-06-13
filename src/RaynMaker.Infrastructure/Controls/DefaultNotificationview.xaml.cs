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

        public Action FinishInteraction { get; set; }

        public INotification Notification
        {
            get { return DataContext as INotification; }
            set { DataContext = value; }
        }

        private void OnOk( object sender, RoutedEventArgs e )
        {
            FinishInteraction();
        }
    }
}
