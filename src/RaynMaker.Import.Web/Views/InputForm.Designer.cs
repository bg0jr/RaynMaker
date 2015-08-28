namespace RaynMaker.Import.Web.Views
{
    partial class InputForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose( bool disposing )
        {
            if ( disposing && ( components != null ) )
            {
                components.Dispose();
            }
            base.Dispose( disposing );
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.myMessage = new System.Windows.Forms.Label();
            this.myInput = new System.Windows.Forms.TextBox();
            this.myCancel = new System.Windows.Forms.Button();
            this.myOk = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // myMessage
            // 
            this.myMessage.AutoSize = true;
            this.myMessage.Location = new System.Drawing.Point( 0, 9 );
            this.myMessage.Name = "myMessage";
            this.myMessage.Size = new System.Drawing.Size( 35, 13 );
            this.myMessage.TabIndex = 0;
            this.myMessage.Text = "Enter:";
            // 
            // myInput
            // 
            this.myInput.Location = new System.Drawing.Point( 3, 25 );
            this.myInput.Name = "myInput";
            this.myInput.Size = new System.Drawing.Size( 277, 20 );
            this.myInput.TabIndex = 1;
            // 
            // myCancel
            // 
            this.myCancel.Location = new System.Drawing.Point( 205, 51 );
            this.myCancel.Name = "myCancel";
            this.myCancel.Size = new System.Drawing.Size( 75, 23 );
            this.myCancel.TabIndex = 2;
            this.myCancel.Text = "Cancel";
            this.myCancel.UseVisualStyleBackColor = true;
            this.myCancel.Click += new System.EventHandler( this.myCancel_Click );
            // 
            // myOk
            // 
            this.myOk.Location = new System.Drawing.Point( 124, 51 );
            this.myOk.Name = "myOk";
            this.myOk.Size = new System.Drawing.Size( 75, 23 );
            this.myOk.TabIndex = 3;
            this.myOk.Text = "Ok";
            this.myOk.UseVisualStyleBackColor = true;
            this.myOk.Click += new System.EventHandler( this.myOk_Click );
            // 
            // InputForm
            // 
            this.AcceptButton = this.myOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size( 284, 79 );
            this.Controls.Add( this.myOk );
            this.Controls.Add( this.myCancel );
            this.Controls.Add( this.myInput );
            this.Controls.Add( this.myMessage );
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "InputForm";
            this.Text = "Input";
            this.ResumeLayout( false );
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label myMessage;
        private System.Windows.Forms.TextBox myInput;
        private System.Windows.Forms.Button myCancel;
        private System.Windows.Forms.Button myOk;
    }
}