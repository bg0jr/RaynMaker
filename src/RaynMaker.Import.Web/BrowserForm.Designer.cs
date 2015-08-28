namespace RaynMaker.Import.Web
{
    partial class BrowserForm
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
            if ( disposing && (components != null) )
            {
                // TODO: does this dispose the browser?
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
            this.components = new System.ComponentModel.Container();
            this.myUrlTxt = new System.Windows.Forms.TextBox();
            this.myBrowser = new System.Windows.Forms.WebBrowser();
            this.myGo = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.myReset = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.myValue = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.mySkipColumns = new System.Windows.Forms.TextBox();
            this.mySkipRows = new System.Windows.Forms.TextBox();
            this.mySeriesName = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.myColumnHeader = new System.Windows.Forms.TextBox();
            this.myRowHeader = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.myDimension = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.mySearchPath = new System.Windows.Forms.Button();
            this.myPath = new System.Windows.Forms.TextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.myEditCapture = new System.Windows.Forms.Button();
            this.myReplay = new System.Windows.Forms.Button();
            this.myNavUrls = new System.Windows.Forms.TextBox();
            this.myCapture = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.myValidateDatumProvidersMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.panel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // myUrlTxt
            // 
            this.myUrlTxt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.myUrlTxt.Location = new System.Drawing.Point(5, 27);
            this.myUrlTxt.Name = "myUrlTxt";
            this.myUrlTxt.Size = new System.Drawing.Size(694, 20);
            this.myUrlTxt.TabIndex = 0;
            // 
            // myBrowser
            // 
            this.myBrowser.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.myBrowser.IsWebBrowserContextMenuEnabled = false;
            this.myBrowser.Location = new System.Drawing.Point(5, 54);
            this.myBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.myBrowser.Name = "myBrowser";
            this.myBrowser.Size = new System.Drawing.Size(775, 364);
            this.myBrowser.TabIndex = 2;
            // 
            // myGo
            // 
            this.myGo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.myGo.Location = new System.Drawing.Point(705, 25);
            this.myGo.Name = "myGo";
            this.myGo.Size = new System.Drawing.Size(75, 23);
            this.myGo.TabIndex = 1;
            this.myGo.Text = "&Go";
            this.myGo.UseVisualStyleBackColor = true;
            this.myGo.Click += new System.EventHandler(this.myGo_Click);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.tabControl1);
            this.panel1.Location = new System.Drawing.Point(5, 424);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(775, 135);
            this.panel1.TabIndex = 4;
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(775, 135);
            this.tabControl1.TabIndex = 22;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.myReset);
            this.tabPage1.Controls.Add(this.label8);
            this.tabPage1.Controls.Add(this.myValue);
            this.tabPage1.Controls.Add(this.label7);
            this.tabPage1.Controls.Add(this.mySkipColumns);
            this.tabPage1.Controls.Add(this.mySkipRows);
            this.tabPage1.Controls.Add(this.mySeriesName);
            this.tabPage1.Controls.Add(this.label6);
            this.tabPage1.Controls.Add(this.label5);
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.myColumnHeader);
            this.tabPage1.Controls.Add(this.myRowHeader);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.myDimension);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.mySearchPath);
            this.tabPage1.Controls.Add(this.myPath);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(767, 109);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Data description";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // myReset
            // 
            this.myReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.myReset.Location = new System.Drawing.Point(607, 83);
            this.myReset.Name = "myReset";
            this.myReset.Size = new System.Drawing.Size(75, 23);
            this.myReset.TabIndex = 39;
            this.myReset.Text = "Reset";
            this.myReset.UseVisualStyleBackColor = true;
            this.myReset.Click += new System.EventHandler(this.myReset_Click);
            // 
            // label8
            // 
            this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(626, 9);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(19, 13);
            this.label8.TabIndex = 38;
            this.label8.Text = "=>";
            // 
            // myValue
            // 
            this.myValue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.myValue.Location = new System.Drawing.Point(651, 6);
            this.myValue.Name = "myValue";
            this.myValue.Size = new System.Drawing.Size(112, 20);
            this.myValue.TabIndex = 37;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(0, 11);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(73, 13);
            this.label7.TabIndex = 36;
            this.label7.Text = "Selected path";
            // 
            // mySkipColumns
            // 
            this.mySkipColumns.Location = new System.Drawing.Point(349, 85);
            this.mySkipColumns.Name = "mySkipColumns";
            this.mySkipColumns.Size = new System.Drawing.Size(62, 20);
            this.mySkipColumns.TabIndex = 35;
            this.mySkipColumns.TextChanged += new System.EventHandler(this.mySkipColumns_TextChanged);
            // 
            // mySkipRows
            // 
            this.mySkipRows.Location = new System.Drawing.Point(349, 59);
            this.mySkipRows.Name = "mySkipRows";
            this.mySkipRows.Size = new System.Drawing.Size(62, 20);
            this.mySkipRows.TabIndex = 34;
            this.mySkipRows.TextChanged += new System.EventHandler(this.mySkipRows_TextChanged);
            // 
            // mySeriesName
            // 
            this.mySeriesName.Location = new System.Drawing.Point(349, 32);
            this.mySeriesName.Name = "mySeriesName";
            this.mySeriesName.Size = new System.Drawing.Size(139, 20);
            this.mySeriesName.TabIndex = 33;
            this.mySeriesName.TextChanged += new System.EventHandler(this.mySeriesName_TextChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(235, 88);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(70, 13);
            this.label6.TabIndex = 32;
            this.label6.Text = "Skip columns";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(235, 35);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(108, 13);
            this.label5.TabIndex = 31;
            this.label5.Text = "Series name contains";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(235, 62);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 13);
            this.label4.TabIndex = 30;
            this.label4.Text = "Skip rows";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(0, 88);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(98, 13);
            this.label3.TabIndex = 29;
            this.label3.Text = "Column header row";
            // 
            // myColumnHeader
            // 
            this.myColumnHeader.Location = new System.Drawing.Point(108, 85);
            this.myColumnHeader.Name = "myColumnHeader";
            this.myColumnHeader.Size = new System.Drawing.Size(62, 20);
            this.myColumnHeader.TabIndex = 28;
            this.myColumnHeader.TextChanged += new System.EventHandler(this.myColumnHeader_TextChanged);
            // 
            // myRowHeader
            // 
            this.myRowHeader.Location = new System.Drawing.Point(108, 59);
            this.myRowHeader.Name = "myRowHeader";
            this.myRowHeader.Size = new System.Drawing.Size(62, 20);
            this.myRowHeader.TabIndex = 27;
            this.myRowHeader.TextChanged += new System.EventHandler(this.myRowHeader_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(0, 62);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(102, 13);
            this.label2.TabIndex = 26;
            this.label2.Text = "Row header column";
            // 
            // myDimension
            // 
            this.myDimension.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.myDimension.FormattingEnabled = true;
            this.myDimension.Items.AddRange(new object[] {
            "None",
            "Row",
            "Column"});
            this.myDimension.Location = new System.Drawing.Point(108, 32);
            this.myDimension.Name = "myDimension";
            this.myDimension.Size = new System.Drawing.Size(115, 21);
            this.myDimension.TabIndex = 25;
            this.myDimension.SelectedIndexChanged += new System.EventHandler(this.myDimension_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(0, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 13);
            this.label1.TabIndex = 24;
            this.label1.Text = "Dimension";
            // 
            // mySearchPath
            // 
            this.mySearchPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.mySearchPath.Location = new System.Drawing.Point(688, 83);
            this.mySearchPath.Name = "mySearchPath";
            this.mySearchPath.Size = new System.Drawing.Size(75, 23);
            this.mySearchPath.TabIndex = 23;
            this.mySearchPath.Text = "Search Path";
            this.mySearchPath.UseVisualStyleBackColor = true;
            this.mySearchPath.Click += new System.EventHandler(this.mySearchPath_Click);
            // 
            // myPath
            // 
            this.myPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.myPath.Location = new System.Drawing.Point(108, 6);
            this.myPath.Name = "myPath";
            this.myPath.Size = new System.Drawing.Size(512, 20);
            this.myPath.TabIndex = 22;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.myEditCapture);
            this.tabPage2.Controls.Add(this.myReplay);
            this.tabPage2.Controls.Add(this.myNavUrls);
            this.tabPage2.Controls.Add(this.myCapture);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(767, 109);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Navigation";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // myEditCapture
            // 
            this.myEditCapture.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.myEditCapture.Location = new System.Drawing.Point(305, 80);
            this.myEditCapture.Name = "myEditCapture";
            this.myEditCapture.Size = new System.Drawing.Size(87, 23);
            this.myEditCapture.TabIndex = 6;
            this.myEditCapture.Text = "Edit...";
            this.myEditCapture.UseVisualStyleBackColor = true;
            this.myEditCapture.Click += new System.EventHandler(this.myEditCapture_Click);
            // 
            // myReplay
            // 
            this.myReplay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.myReplay.Location = new System.Drawing.Point(305, 35);
            this.myReplay.Name = "myReplay";
            this.myReplay.Size = new System.Drawing.Size(87, 23);
            this.myReplay.TabIndex = 5;
            this.myReplay.Text = "Replay";
            this.myReplay.UseVisualStyleBackColor = true;
            this.myReplay.Click += new System.EventHandler(this.myReplay_Click);
            // 
            // myNavUrls
            // 
            this.myNavUrls.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.myNavUrls.Location = new System.Drawing.Point(0, 6);
            this.myNavUrls.Multiline = true;
            this.myNavUrls.Name = "myNavUrls";
            this.myNavUrls.Size = new System.Drawing.Size(299, 100);
            this.myNavUrls.TabIndex = 4;
            this.myNavUrls.WordWrap = false;
            // 
            // myCapture
            // 
            this.myCapture.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.myCapture.Location = new System.Drawing.Point(305, 6);
            this.myCapture.Name = "myCapture";
            this.myCapture.Size = new System.Drawing.Size(87, 23);
            this.myCapture.TabIndex = 1;
            this.myCapture.Text = "Start capturing";
            this.myCapture.UseVisualStyleBackColor = true;
            this.myCapture.Click += new System.EventHandler(this.myCapture_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.SystemColors.Menu;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem2});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(784, 24);
            this.menuStrip1.TabIndex = 5;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.myValidateDatumProvidersMenu});
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(48, 20);
            this.toolStripMenuItem2.Text = "&Tools";
            // 
            // myValidateDatumProvidersMenu
            // 
            this.myValidateDatumProvidersMenu.Name = "myValidateDatumProvidersMenu";
            this.myValidateDatumProvidersMenu.Size = new System.Drawing.Size(211, 22);
            this.myValidateDatumProvidersMenu.Text = "Validate datum locators ...";
            this.myValidateDatumProvidersMenu.Click += new System.EventHandler(this.myValidateDatumLocatorsMenu_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(32, 19);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // BrowserForm
            // 
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.myBrowser);
            this.Controls.Add(this.myGo);
            this.Controls.Add(this.myUrlTxt);
            this.Controls.Add(this.menuStrip1);
            this.MinimumSize = new System.Drawing.Size(784, 562);
            this.Name = "BrowserForm";
            this.Size = new System.Drawing.Size(0, 0);
            this.panel1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox myUrlTxt;
        private System.Windows.Forms.WebBrowser myBrowser;
        private System.Windows.Forms.Button myGo;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Button myReset;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox myValue;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox mySkipColumns;
        private System.Windows.Forms.TextBox mySkipRows;
        private System.Windows.Forms.TextBox mySeriesName;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox myColumnHeader;
        private System.Windows.Forms.TextBox myRowHeader;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox myDimension;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button mySearchPath;
        private System.Windows.Forms.TextBox myPath;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button myCapture;
        private System.Windows.Forms.TextBox myNavUrls;
        private System.Windows.Forms.Button myReplay;
        private System.Windows.Forms.Button myEditCapture;
        private System.Windows.Forms.ToolStripMenuItem myValidateDatumProvidersMenu;
    }
}