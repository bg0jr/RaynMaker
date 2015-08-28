using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RaynMaker.Import.Web.Views
{
    public partial class EditCaptureForm : Form
    {
        public EditCaptureForm()
        {
            InitializeComponent();

            myMacros.Items.Add( "${stock.Isin}" );
            myMacros.Items.Add( "${stock.Wpkn}" );
            myMacros.Items.Add( "${stock.Symbol}" );
            myMacros.SelectedIndex = 0;
        }

        public string NavUrls
        {
            get { return myNavUrls.Text; }
            set { myNavUrls.Text = value; }
        }

        private void myInsertMacro_Click( object sender, EventArgs e )
        {
            myNavUrls.SelectedText = myMacros.SelectedItem.ToString();
        }

        private void myOk_Click( object sender, EventArgs e )
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void myCancel_Click( object sender, EventArgs e )
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void myInsertRegExp_Click( object sender, EventArgs e )
        {
            myNavUrls.SelectedText = "{" + myRegExp.Text + "}";
        }

        private void myInsertRegExpTarget_Click( object sender, EventArgs e )
        {
            myNavUrls.SelectedText = myRegExpTarget.Text;
        }
    }
}
