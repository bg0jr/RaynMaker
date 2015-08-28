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
    public partial class InputForm : Form
    {
        public InputForm()
        {
            InitializeComponent();
        }

        public static string Show( string title, string message )
        {
            InputForm form = new InputForm();

            form.Text = title;
            form.myMessage.Text = message;

            if ( form.ShowDialog() == DialogResult.OK )
            {
                return form.Input;
            }
            else
            {
                return null;
            }
        }

        public string Input
        {
            get { return myInput.Text; }
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
    }
}
