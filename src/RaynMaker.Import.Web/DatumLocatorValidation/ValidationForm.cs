using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using RaynMaker.Import;
using RaynMaker.Import.DatumLocators;
using RaynMaker.Import.Web.DatumLocatorValidation;

namespace RaynMaker.Import.Web.DatumLocationValidation
{
    public partial class ValidationForm : Form
    {
        private ValidationController myController;
        private IBrowser myBrowser;

        public ValidationForm( IBrowser browser )
        {
            myBrowser=browser;

            InitializeComponent();

            myController = new ValidationController();

            myDatumLocators.View = View.List;
        }

        private void DatumProviderValidation_Load( object sender, EventArgs e )
        {
            var datumLocators = myController.GetDatumLocators();
            foreach ( var locator in datumLocators )
            {
                myDatumLocators.Items.Add( new LocatorListViewItem( locator ) );
            }
        }

        private void myCancelBtn_Click( object sender, EventArgs e )
        {
            Close();
        }

        private void myRunBtn_Click( object sender, EventArgs e )
        {
            myValidateSelected.Enabled = false;

            ValidateItems( myDatumLocators.SelectedItems.Cast<LocatorListViewItem>() );

            myValidateSelected.Enabled = true;
        }

        private void myValidateAll_Click( object sender, EventArgs e )
        {
            myValidateAll.Enabled = false;

            ValidateItems( myDatumLocators.Items.Cast<LocatorListViewItem>() );

            myValidateAll.Enabled = true;
        }

        private void ValidateItems( IEnumerable<LocatorListViewItem> items )
        {
            foreach ( var item in items )
            {
                item.BackColor = myDatumLocators.BackColor;
            }

            foreach ( var item in items )
            {
                item.BackColor = Color.LightBlue;

                item.Content.ValidationResult = myController.Validate( item.Content.DatumLocator );

                if ( item.Content.ValidationResult.Succeeded )
                {
                    item.BackColor = Color.Green;
                }
                else
                {
                    item.BackColor = Color.Red;
                }
            }

            UpdateSelectedItemProperties();
        }

        private void myDatumLocators_SelectedIndexChanged( object sender, EventArgs e )
        {
            UpdateSelectedItemProperties();
        }

        private void UpdateSelectedItemProperties()
        {
            var selecteditem = myDatumLocators.SelectedItems
                .Cast<LocatorListViewItem>()
                .FirstOrDefault();

            if ( selecteditem == null )
            {
                return;
            }

            myItemProperties.SelectedObject = selecteditem.Content;
        }

        private void myNavigateMenu_Click( object sender, EventArgs e )
        {
            myBrowser.Navigate( myItemProperties.SelectedGridItem.Value.ToString() );
        }

        private void myPropertiesContextMenu_Opening( object sender, CancelEventArgs e )
        {
            myNavigateMenu.Enabled = false;

            if ( myItemProperties.SelectedGridItem.Label == "DocumentLocation" )
            {
                myNavigateMenu.Enabled = true;
            }
        }
    }
}
