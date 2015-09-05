using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Microsoft.Practices.Prism.Mvvm;
using Plainion.Prism.Interactivity.InteractionRequest;
using Plainion.Windows.Controls;
using RaynMaker.Import.Spec;

namespace RaynMaker.Import.Web.ViewModels
{
    [Export]
    class EditCaptureViewModel : BindableBase, IInteractionRequestAware
    {
        private IEnumerable<NavigatorUrl> myUrls;
        private NavigatorUrl mySelectedUrl;
        private string mySelectedMacro;
        private string myRegExp;
        private string myRegExpTarget;
        private string mySelectedText;
        private INotification myNotification;

        public EditCaptureViewModel()
        {
            RegExpTarget = "{0}";
            SelectedMacro = Macros.First();

            InsertMacroCommand = new DelegateCommand( OnInsertMacro );
            InsertRegExpCommand = new DelegateCommand( OnInsertRegExp );
            InsertRegExpTargetCommand = new DelegateCommand( OnInsertRegExpTarget );

            OkCommand = new DelegateCommand( OnOk );
            CancelCommand = new DelegateCommand( OnCancel );
        }

        public IEnumerable<NavigatorUrl> Urls
        {
            get { return myUrls; }
            set { SetProperty( ref myUrls, value ); }
        }

        public NavigatorUrl SelectedUrl
        {
            get { return mySelectedUrl; }
            set { SetProperty( ref mySelectedUrl, value ); }
        }

        public string SelectedText
        {
            get { return mySelectedText; }
            set { SetProperty( ref mySelectedText, value ); }
        }

        public IEnumerable<string> Macros { get { return new[] { "${stock.Isin}", "${stock.Wpkn}", "${stock.Symbol}" }; } }

        public string SelectedMacro
        {
            get { return mySelectedMacro; }
            set { SetProperty( ref mySelectedMacro, value ); }
        }

        public ICommand InsertMacroCommand { get; private set; }

        private void OnInsertMacro()
        {
            SelectedText = SelectedMacro;
        }

        public string RegExp
        {
            get { return myRegExp; }
            set { SetProperty( ref myRegExp, value ); }
        }

        public ICommand InsertRegExpCommand { get; private set; }

        private void OnInsertRegExp()
        {
            SelectedText = "{" + RegExp + "}";
        }

        public string RegExpTarget
        {
            get { return myRegExpTarget; }
            set { SetProperty( ref myRegExpTarget, value ); }
        }

        public ICommand InsertRegExpTargetCommand { get; private set; }

        private void OnInsertRegExpTarget()
        {
            SelectedText = RegExpTarget;
        }

        public ICommand OkCommand { get; private set; }

        private void OnOk()
        {
            TextBoxBinding.ForceSourceUpdate();

            Notification.TrySetConfirmed( true );

            Notification.Content = Urls;

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
                Urls = ( IEnumerable<NavigatorUrl> )myNotification.Content;
            }
        }
    }
}
