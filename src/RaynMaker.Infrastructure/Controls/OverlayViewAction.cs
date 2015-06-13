using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using System.Windows.Markup;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Plainion;

namespace RaynMaker.Infrastructure.Controls
{
    /// <summary>
    /// Displays a view in a "dialog-style" by just adding it to a container instead of using a new window.
    /// If this container is e.g. a Grid the previews content will just be "covered".
    /// </summary>
    [DefaultProperty( "ViewContent" ), ContentProperty( "ViewContent" )]
    public class OverlayViewAction : TriggerAction<FrameworkElement>
    {
        /// <summary>
        /// The view to display in this overlay.
        /// </summary>
        public static readonly DependencyProperty ViewContentProperty = DependencyProperty.Register( "ViewContent",
            typeof( FrameworkElement ), typeof( OverlayViewAction ), new PropertyMetadata( null ) );

        public FrameworkElement ViewContent
        {
            get { return ( FrameworkElement )GetValue( OverlayViewAction.ViewContentProperty ); }
            set { SetValue( OverlayViewAction.ViewContentProperty, value ); }
        }

        /// <summary>
        /// The Panel to which the view should be added
        /// </summary>
        public static readonly DependencyProperty ContainerProperty = DependencyProperty.Register( "Container",
            typeof( FrameworkElement ), typeof( OverlayViewAction ), new PropertyMetadata( null ) );

        public Panel Container
        {
            get { return ( Panel )GetValue( OverlayViewAction.ContainerProperty ); }
            set { SetValue( OverlayViewAction.ContainerProperty, value ); }
        }

        /// <summary>
        /// Style of the overlay the view will be placed into.
        /// </summary>
        public static readonly DependencyProperty StyleProperty = DependencyProperty.Register( "Style",
            typeof( Style ), typeof( OverlayViewAction ), new PropertyMetadata( null ) );

        public Style Style
        {
            get { return ( Style )GetValue( OverlayViewAction.StyleProperty ); }
            set { SetValue( OverlayViewAction.StyleProperty, value ); }
        }

        protected override void Invoke( object parameter )
        {
            var args = parameter as InteractionRequestedEventArgs;
            Contract.RequiresNotNull( args, "parameter has to be InteractionRequestedEventArgs" );

            Contract.RequiresNotNull( Container, "Container" );

            var view = GetOverlayView( args.Context );

            var interactionRequestAware = GetInteractionRequestAware( view );

            Contract.Requires( interactionRequestAware != null, "View or DataContext has to implement IInteractionRequestAware" );

            interactionRequestAware.Notification = args.Context;
            interactionRequestAware.FinishInteraction = () =>
            {
                Container.Children.Remove( view );

                args.Callback();
            };


            Container.Children.Add( view );
        }

        private ContentControl GetOverlayView( INotification notification )
        {
            var host = new ContentControl();
            host.DataContext = notification;

            if( ViewContent != null )
            {
                host.Content = ViewContent;
            }
            else if( notification is IConfirmation )
            {
                host.Content = new DefaultConfirmationView();
            }
            else
            {
                host.Content = new DefaultNotificationView();
            }

            if( Style != null )
            {
                host.Style = Style;
            }

            return host;
        }

        private static IInteractionRequestAware GetInteractionRequestAware( ContentControl host )
        {
            var interactionRequestAware = host.Content as IInteractionRequestAware;
            if( interactionRequestAware != null )
            {
                return interactionRequestAware;
            }

            var frameworkElement = host.Content as FrameworkElement;
            if( frameworkElement != null )
            {
                return frameworkElement.DataContext as IInteractionRequestAware;
            }

            return null;
        }
    }
}
