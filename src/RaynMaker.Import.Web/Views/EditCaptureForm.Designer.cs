namespace RaynMaker.Import.Web.Views
{
    partial class EditCaptureForm
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
            this.myNavUrls = new System.Windows.Forms.TextBox();
            this.myInsertMacro = new System.Windows.Forms.Button();
            this.myMacros = new System.Windows.Forms.ComboBox();
            this.myOk = new System.Windows.Forms.Button();
            this.myCancel = new System.Windows.Forms.Button();
            this.myRegExp = new System.Windows.Forms.TextBox();
            this.myInsertRegExp = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.myInsertRegExpTarget = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.myRegExpTarget = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // myNavUrls
            // 
            this.myNavUrls.Anchor = ( (System.Windows.Forms.AnchorStyles)( ( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom )
                        | System.Windows.Forms.AnchorStyles.Left )
                        | System.Windows.Forms.AnchorStyles.Right ) ) );
            this.myNavUrls.Location = new System.Drawing.Point( 3, 5 );
            this.myNavUrls.Multiline = true;
            this.myNavUrls.Name = "myNavUrls";
            this.myNavUrls.Size = new System.Drawing.Size( 472, 99 );
            this.myNavUrls.TabIndex = 0;
            this.myNavUrls.WordWrap = false;
            // 
            // myInsertMacro
            // 
            this.myInsertMacro.Anchor = ( (System.Windows.Forms.AnchorStyles)( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left ) ) );
            this.myInsertMacro.Location = new System.Drawing.Point( 236, 110 );
            this.myInsertMacro.Name = "myInsertMacro";
            this.myInsertMacro.Size = new System.Drawing.Size( 114, 23 );
            this.myInsertMacro.TabIndex = 1;
            this.myInsertMacro.Text = "Insert macro";
            this.myInsertMacro.UseVisualStyleBackColor = true;
            this.myInsertMacro.Click += new System.EventHandler( this.myInsertMacro_Click );
            // 
            // myMacros
            // 
            this.myMacros.Anchor = ( (System.Windows.Forms.AnchorStyles)( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left ) ) );
            this.myMacros.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.myMacros.FormattingEnabled = true;
            this.myMacros.Location = new System.Drawing.Point( 81, 112 );
            this.myMacros.Name = "myMacros";
            this.myMacros.Size = new System.Drawing.Size( 149, 21 );
            this.myMacros.TabIndex = 2;
            // 
            // myOk
            // 
            this.myOk.Anchor = ( (System.Windows.Forms.AnchorStyles)( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right ) ) );
            this.myOk.Location = new System.Drawing.Point( 319, 206 );
            this.myOk.Name = "myOk";
            this.myOk.Size = new System.Drawing.Size( 75, 23 );
            this.myOk.TabIndex = 3;
            this.myOk.Text = "Ok";
            this.myOk.UseVisualStyleBackColor = true;
            this.myOk.Click += new System.EventHandler( this.myOk_Click );
            // 
            // myCancel
            // 
            this.myCancel.Anchor = ( (System.Windows.Forms.AnchorStyles)( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right ) ) );
            this.myCancel.Location = new System.Drawing.Point( 400, 206 );
            this.myCancel.Name = "myCancel";
            this.myCancel.Size = new System.Drawing.Size( 75, 23 );
            this.myCancel.TabIndex = 4;
            this.myCancel.Text = "Cancel";
            this.myCancel.UseVisualStyleBackColor = true;
            this.myCancel.Click += new System.EventHandler( this.myCancel_Click );
            // 
            // myRegExp
            // 
            this.myRegExp.Anchor = ( (System.Windows.Forms.AnchorStyles)( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left ) ) );
            this.myRegExp.Location = new System.Drawing.Point( 81, 141 );
            this.myRegExp.Name = "myRegExp";
            this.myRegExp.Size = new System.Drawing.Size( 149, 20 );
            this.myRegExp.TabIndex = 5;
            // 
            // myInsertRegExp
            // 
            this.myInsertRegExp.Anchor = ( (System.Windows.Forms.AnchorStyles)( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left ) ) );
            this.myInsertRegExp.Location = new System.Drawing.Point( 236, 139 );
            this.myInsertRegExp.Name = "myInsertRegExp";
            this.myInsertRegExp.Size = new System.Drawing.Size( 114, 23 );
            this.myInsertRegExp.TabIndex = 6;
            this.myInsertRegExp.Text = "Insert RegExp";
            this.myInsertRegExp.UseVisualStyleBackColor = true;
            this.myInsertRegExp.Click += new System.EventHandler( this.myInsertRegExp_Click );
            // 
            // label1
            // 
            this.label1.Anchor = ( (System.Windows.Forms.AnchorStyles)( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left ) ) );
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point( 0, 115 );
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size( 42, 13 );
            this.label1.TabIndex = 7;
            this.label1.Text = "Macros";
            // 
            // label2
            // 
            this.label2.Anchor = ( (System.Windows.Forms.AnchorStyles)( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left ) ) );
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point( 0, 144 );
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size( 45, 13 );
            this.label2.TabIndex = 8;
            this.label2.Text = "RegExp";
            // 
            // myInsertRegExpTarget
            // 
            this.myInsertRegExpTarget.Anchor = ( (System.Windows.Forms.AnchorStyles)( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left ) ) );
            this.myInsertRegExpTarget.Location = new System.Drawing.Point( 236, 168 );
            this.myInsertRegExpTarget.Name = "myInsertRegExpTarget";
            this.myInsertRegExpTarget.Size = new System.Drawing.Size( 114, 23 );
            this.myInsertRegExpTarget.TabIndex = 9;
            this.myInsertRegExpTarget.Text = "Insert RegExp target";
            this.myInsertRegExpTarget.UseVisualStyleBackColor = true;
            this.myInsertRegExpTarget.Click += new System.EventHandler( this.myInsertRegExpTarget_Click );
            // 
            // label3
            // 
            this.label3.Anchor = ( (System.Windows.Forms.AnchorStyles)( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left ) ) );
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point( 0, 173 );
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size( 75, 13 );
            this.label3.TabIndex = 10;
            this.label3.Text = "RegExp target";
            // 
            // myRegExpTarget
            // 
            this.myRegExpTarget.Anchor = ( (System.Windows.Forms.AnchorStyles)( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left ) ) );
            this.myRegExpTarget.BackColor = System.Drawing.SystemColors.Control;
            this.myRegExpTarget.Enabled = false;
            this.myRegExpTarget.Location = new System.Drawing.Point( 81, 170 );
            this.myRegExpTarget.Name = "myRegExpTarget";
            this.myRegExpTarget.Size = new System.Drawing.Size( 149, 20 );
            this.myRegExpTarget.TabIndex = 11;
            this.myRegExpTarget.Text = "{0}";
            // 
            // EditCaptureForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size( 478, 233 );
            this.Controls.Add( this.myRegExpTarget );
            this.Controls.Add( this.label3 );
            this.Controls.Add( this.myInsertRegExpTarget );
            this.Controls.Add( this.label2 );
            this.Controls.Add( this.label1 );
            this.Controls.Add( this.myInsertRegExp );
            this.Controls.Add( this.myRegExp );
            this.Controls.Add( this.myCancel );
            this.Controls.Add( this.myOk );
            this.Controls.Add( this.myMacros );
            this.Controls.Add( this.myInsertMacro );
            this.Controls.Add( this.myNavUrls );
            this.Name = "EditCaptureForm";
            this.Text = "Edit";
            this.ResumeLayout( false );
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox myNavUrls;
        private System.Windows.Forms.Button myInsertMacro;
        private System.Windows.Forms.ComboBox myMacros;
        private System.Windows.Forms.Button myOk;
        private System.Windows.Forms.Button myCancel;
        private System.Windows.Forms.TextBox myRegExp;
        private System.Windows.Forms.Button myInsertRegExp;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button myInsertRegExpTarget;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox myRegExpTarget;
    }
}