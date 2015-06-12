using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using System.Windows.Markup;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;

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
            InteractionRequestedEventArgs args = parameter as InteractionRequestedEventArgs;
            if( args == null )
            {
                return;
            }

            if( ViewContent == null || Container == null )
            {
                return;
            }

            var host = CreateViewHost( args.Context );

            // TODO: as we have conntected host and ViewContent already - in case ViewContent does not bring its own
            // DataContext do we already inherit the one from host (which is the notification)?

            var interactionRequestAware = ViewContent.DataContext as IInteractionRequestAware;
            if( interactionRequestAware != null )
            {
                interactionRequestAware.Notification = args.Context;
                interactionRequestAware.FinishInteraction = () =>
                {
                    Container.Children.Remove( host );

                    args.Callback();
                };
            }

            Container.Children.Add( host );
        }

        private ContentControl CreateViewHost( INotification notification )
        {
            var host = new ContentControl();
            host.DataContext = notification;
            host.Content = ViewContent;

            if( Style != null )
            {
                host.Style = Style;
            }

            return host;
        }
    }
}
