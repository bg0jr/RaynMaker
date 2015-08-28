namespace RaynMaker.Import.Web.DatumLocationValidation
{
    partial class ValidationForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.myDatumLocators = new System.Windows.Forms.ListView();
            this.myValidateSelected = new System.Windows.Forms.Button();
            this.myCancelBtn = new System.Windows.Forms.Button();
            this.myValidateAll = new System.Windows.Forms.Button();
            this.myItemProperties = new System.Windows.Forms.PropertyGrid();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.myPropertiesContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.myNavigateMenu = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.myPropertiesContextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // myDatumLocators
            // 
            this.myDatumLocators.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.myDatumLocators.Location = new System.Drawing.Point(0, 0);
            this.myDatumLocators.Name = "myDatumLocators";
            this.myDatumLocators.Size = new System.Drawing.Size(147, 390);
            this.myDatumLocators.TabIndex = 0;
            this.myDatumLocators.UseCompatibleStateImageBehavior = false;
            this.myDatumLocators.SelectedIndexChanged += new System.EventHandler(this.myDatumLocators_SelectedIndexChanged);
            // 
            // myValidateSelected
            // 
            this.myValidateSelected.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.myValidateSelected.Location = new System.Drawing.Point(417, 408);
            this.myValidateSelected.Name = "myValidateSelected";
            this.myValidateSelected.Size = new System.Drawing.Size(116, 23);
            this.myValidateSelected.TabIndex = 1;
            this.myValidateSelected.Text = "Validate selected";
            this.myValidateSelected.UseVisualStyleBackColor = true;
            this.myValidateSelected.Click += new System.EventHandler(this.myRunBtn_Click);
            // 
            // myCancelBtn
            // 
            this.myCancelBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.myCancelBtn.Location = new System.Drawing.Point(539, 408);
            this.myCancelBtn.Name = "myCancelBtn";
            this.myCancelBtn.Size = new System.Drawing.Size(75, 23);
            this.myCancelBtn.TabIndex = 2;
            this.myCancelBtn.Text = "Cancel";
            this.myCancelBtn.UseVisualStyleBackColor = true;
            this.myCancelBtn.Click += new System.EventHandler(this.myCancelBtn_Click);
            // 
            // myValidateAll
            // 
            this.myValidateAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.myValidateAll.Location = new System.Drawing.Point(305, 408);
            this.myValidateAll.Name = "myValidateAll";
            this.myValidateAll.Size = new System.Drawing.Size(106, 23);
            this.myValidateAll.TabIndex = 3;
            this.myValidateAll.Text = "Validate all";
            this.myValidateAll.UseVisualStyleBackColor = true;
            this.myValidateAll.Click += new System.EventHandler(this.myValidateAll_Click);
            // 
            // myItemProperties
            // 
            this.myItemProperties.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.myItemProperties.ContextMenuStrip = this.myPropertiesContextMenu;
            this.myItemProperties.HelpVisible = false;
            this.myItemProperties.Location = new System.Drawing.Point(3, 0);
            this.myItemProperties.Name = "myItemProperties";
            this.myItemProperties.Size = new System.Drawing.Size(445, 390);
            this.myItemProperties.TabIndex = 4;
            this.myItemProperties.ToolbarVisible = false;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(12, 12);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.myDatumLocators);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.myItemProperties);
            this.splitContainer1.Size = new System.Drawing.Size(602, 390);
            this.splitContainer1.SplitterDistance = 150;
            this.splitContainer1.TabIndex = 5;
            // 
            // myPropertiesContextMenu
            // 
            this.myPropertiesContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.myNavigateMenu});
            this.myPropertiesContextMenu.Name = "myPropertiesContextMenu";
            this.myPropertiesContextMenu.Size = new System.Drawing.Size(153, 48);
            this.myPropertiesContextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.myPropertiesContextMenu_Opening);
            // 
            // myNavigateMenu
            // 
            this.myNavigateMenu.Name = "myNavigateMenu";
            this.myNavigateMenu.Size = new System.Drawing.Size(152, 22);
            this.myNavigateMenu.Text = "Navigate";
            this.myNavigateMenu.Click += new System.EventHandler(this.myNavigateMenu_Click);
            // 
            // ValidationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(626, 443);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.myValidateAll);
            this.Controls.Add(this.myCancelBtn);
            this.Controls.Add(this.myValidateSelected);
            this.Name = "ValidationForm";
            this.Text = "Datum Locator Validation";
            this.Load += new System.EventHandler(this.DatumProviderValidation_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.myPropertiesContextMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView myDatumLocators;
        private System.Windows.Forms.Button myValidateSelected;
        private System.Windows.Forms.Button myCancelBtn;
        private System.Windows.Forms.Button myValidateAll;
        private System.Windows.Forms.PropertyGrid myItemProperties;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ContextMenuStrip myPropertiesContextMenu;
        private System.Windows.Forms.ToolStripMenuItem myNavigateMenu;
    }
}