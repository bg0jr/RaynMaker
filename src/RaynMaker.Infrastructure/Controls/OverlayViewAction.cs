using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Interactivity;
using System.Windows.Markup;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Plainion;
using Plainion.Windows;

namespace RaynMaker.Infrastructure.Controls
{
    /// <summary>
    /// Displays a view in a "dialog-style" by just adding it to a container instead of using a new window.
    /// If this container is e.g. a Grid the previews content will just be "covered".
    /// 
    /// Set name of the control to "InitialFocus" which should have the initial focus after the view is loaded.
    /// </summary>
    [DefaultProperty( "ViewContent" ), ContentProperty( "ViewContent" )]
    public class OverlayViewAction : TriggerAction<FrameworkElement>
    {
        private FrameworkElementAdorner myAdorner;
        private ContentControl myHost;

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
        /// The element to which the view should be added
        /// </summary>
        public static readonly DependencyProperty ContainerProperty = DependencyProperty.Register( "Container",
            typeof( FrameworkElement ), typeof( OverlayViewAction ), new PropertyMetadata( null ) );

        public FrameworkElement Container
        {
            get { return ( FrameworkElement )GetValue( OverlayViewAction.ContainerProperty ); }
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

            myHost = GetOverlayView( args.Context );

            var interactionRequestAware = GetInteractionRequestAware( myHost );

            Contract.Requires( interactionRequestAware != null, "View or DataContext has to implement IInteractionRequestAware" );

            interactionRequestAware.Notification = args.Context;
            interactionRequestAware.FinishInteraction = () =>
            {
                var adornerLayer = AdornerLayer.GetAdornerLayer( Container );
                adornerLayer.Remove( myAdorner );
                myAdorner = null;

                myHost.Content = null;
                myHost = null;

                args.Callback();
            };

            {
                var adornerLayer = AdornerLayer.GetAdornerLayer( Container );
                myAdorner = new FrameworkElementAdorner( myHost, Container );
                adornerLayer.Add( myAdorner );
            }

            if( myHost.IsLoaded )
            {
                SetInitialFocus( myHost );
            }
            else
            {
                myHost.Loaded += OnViewLoaded;
            }
        }

        void OnViewLoaded( object sender, RoutedEventArgs e )
        {
            myHost.Loaded -= OnViewLoaded;

            SetInitialFocus( myHost );
        }
        
        private static void SetInitialFocus( FrameworkElement view )
        {
            var child = LogicalTreeHelper.FindLogicalNode( view, "InitialFocus" ) as FrameworkElement;
            if( child != null )
            {
                child.Focus();
                return;
            }

            // set focus to view as fallback so that the overlay gets the keyboard input
            // and really acts as kind of modal dialog
            view.Focusable = true;
            view.Focus();
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
