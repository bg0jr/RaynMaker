using System;
using System.ComponentModel.Composition;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Microsoft.Practices.Prism.Mvvm;
using Plainion.Prism.Interactivity.InteractionRequest;
using Plainion.Windows.Controls;

namespace RaynMaker.Import.Web.ViewModels
{
    [Export]
    class InputMacroValueViewModel : BindableBase, IInteractionRequestAware
    {
        private string myMacro;
        private string myValue;
        private INotification myNotification;

        public InputMacroValueViewModel()
        {
            OkCommand = new DelegateCommand( OnOk );
            CancelCommand = new DelegateCommand( OnCancel );
        }

        public string Macro
        {
            get { return myMacro; }
            set { SetProperty( ref myMacro, value ); }
        }

        public string Value
        {
            get { return myValue; }
            set { SetProperty( ref myValue, value ); }
        }

        public ICommand OkCommand { get; private set; }

        private void OnOk()
        {
            TextBoxBinding.ForceSourceUpdate();

            Notification.TrySetConfirmed( true );

            Notification.Content = Value;

            FinishInteraction();
        }

        public ICommand CancelCommand { get; private set; }

        private void OnCancel()
        {
            Notification.TrySetConfirmed( false );
            FinishInteraction();
        }

        public Action FinishInteraction { get; set; }

        public INotification Notification
        {
            get { return myNotification; }
            set
            {
                myNotification = value;
                Macro = ( string )myNotification.Content;
            }
        }
    }
}
