using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Interactivity;
using System.Windows.Markup;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;

namespace RaynMaker.Browser.Controls
{
    [DefaultProperty( "ViewContent" ), ContentProperty( "ViewContent" )]
    public class OverlayViewAction : TriggerAction<FrameworkElement>
    {
        public static readonly DependencyProperty ViewContentProperty = DependencyProperty.Register( "ViewContent",
            typeof( FrameworkElement ), typeof( OverlayViewAction ), new PropertyMetadata( null ) );

        public FrameworkElement ViewContent
        {
            get { return ( FrameworkElement )GetValue( OverlayViewAction.ViewContentProperty ); }
            set { SetValue( OverlayViewAction.ViewContentProperty, value ); }
        }

        public static readonly DependencyProperty OverlayTargetProperty = DependencyProperty.Register( "OverlayTarget",
            typeof( FrameworkElement ), typeof( OverlayViewAction ), new PropertyMetadata( null ) );

        public Panel OverlayTarget
        {
            get { return ( Panel )GetValue( OverlayViewAction.OverlayTargetProperty ); }
            set { SetValue( OverlayViewAction.OverlayTargetProperty, value ); }
        }

        protected override void Invoke( object parameter )
        {
            InteractionRequestedEventArgs args = parameter as InteractionRequestedEventArgs;
            if( args == null )
            {
                return;
            }

            if( ViewContent == null || OverlayTarget == null )
            {
                return;
            }

            var content = ViewContent;

            var interactionRequestAware = ViewContent.DataContext as IInteractionRequestAware;
            if( interactionRequestAware != null )
            {
                interactionRequestAware.Notification = args.Context;
                interactionRequestAware.FinishInteraction = () =>
                {
                    OverlayTarget.Children.Remove( content );

                    args.Callback();
                };
            }

            OverlayTarget.Children.Add( content );
        }
    }
}
