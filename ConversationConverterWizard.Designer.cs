namespace IEConversationConvert
{
    partial class ConversationConverterWizard
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
            this.selectFileWizardPage = new Crownwood.DotNetMagic.Controls.WizardPage();
            this.tlkFileLabel = new System.Windows.Forms.Label();
            this.tlkFileBrowseButton = new System.Windows.Forms.Button();
            this.dialogFileBrowseButton = new System.Windows.Forms.Button();
            this.dialogFileNameTextBox = new System.Windows.Forms.TextBox();
            this.convertWizardPage = new Crownwood.DotNetMagic.Controls.WizardPage();
            this.startConversionButton = new System.Windows.Forms.Button();
            this.conversionLogLabel = new System.Windows.Forms.Label();
            this.conversionReportTextBox = new System.Windows.Forms.TextBox();
            this.dlgFileLabel = new System.Windows.Forms.Label();
            this.tlkFileNameComboBox = new System.Windows.Forms.ComboBox();
            this.selectFileWizardPage.SuspendLayout();
            this.convertWizardPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // WizardControlField
            // 
            // 
            // 
            // 
            this.WizardControlField.HeaderPanel.BackColor = System.Drawing.SystemColors.Window;
            this.WizardControlField.HeaderPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.WizardControlField.HeaderPanel.Location = new System.Drawing.Point(0, 0);
            this.WizardControlField.HeaderPanel.Name = "_panelTop";
            this.WizardControlField.HeaderPanel.Size = new System.Drawing.Size(545, 57);
            this.WizardControlField.HeaderPanel.TabIndex = 1;
            this.WizardControlField.SelectedIndex = 0;
            this.WizardControlField.Size = new System.Drawing.Size(545, 405);
            this.WizardControlField.Style = Crownwood.DotNetMagic.Common.VisualStyle.IDE2005;
            this.WizardControlField.Title = "Infinity Engine conversation converter wizard";
            // 
            // 
            // 
            this.WizardControlField.TrailerPanel.BackColor = System.Drawing.SystemColors.Control;
            this.WizardControlField.TrailerPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.WizardControlField.TrailerPanel.Location = new System.Drawing.Point(0, 357);
            this.WizardControlField.TrailerPanel.Name = "_panelBottom";
            this.WizardControlField.TrailerPanel.Size = new System.Drawing.Size(545, 48);
            this.WizardControlField.TrailerPanel.TabIndex = 2;
            this.WizardControlField.WizardPages.AddRange(new Crownwood.DotNetMagic.Controls.WizardPage[] {
            this.selectFileWizardPage,
            this.convertWizardPage});
            this.WizardControlField.NextClick += new System.ComponentModel.CancelEventHandler(this.WizardControlField_NextClick);
            // 
            // selectFileWizardPage
            // 
            this.selectFileWizardPage.CaptionTitle = "Select dialog file";
            this.selectFileWizardPage.Controls.Add(this.tlkFileNameComboBox);
            this.selectFileWizardPage.Controls.Add(this.dlgFileLabel);
            this.selectFileWizardPage.Controls.Add(this.tlkFileLabel);
            this.selectFileWizardPage.Controls.Add(this.tlkFileBrowseButton);
            this.selectFileWizardPage.Controls.Add(this.dialogFileBrowseButton);
            this.selectFileWizardPage.Controls.Add(this.dialogFileNameTextBox);
            this.selectFileWizardPage.FullPage = false;
            this.selectFileWizardPage.InactiveBackColor = System.Drawing.Color.Empty;
            this.selectFileWizardPage.InactiveTextBackColor = System.Drawing.Color.Empty;
            this.selectFileWizardPage.InactiveTextColor = System.Drawing.Color.Empty;
            this.selectFileWizardPage.Location = new System.Drawing.Point(0, 0);
            this.selectFileWizardPage.Name = "selectFileWizardPage";
            this.selectFileWizardPage.PageDimmed = false;
            this.selectFileWizardPage.SelectBackColor = System.Drawing.Color.Empty;
            this.selectFileWizardPage.SelectTextBackColor = System.Drawing.Color.Empty;
            this.selectFileWizardPage.SelectTextColor = System.Drawing.Color.Empty;
            this.selectFileWizardPage.Size = new System.Drawing.Size(545, 300);
            this.selectFileWizardPage.SubTitle = "Select the Infinity Engine dialog file to convert";
            this.selectFileWizardPage.TabIndex = 4;
            this.selectFileWizardPage.ToolTip = "Page";
            // 
            // tlkFileLabel
            // 
            this.tlkFileLabel.AutoSize = true;
            this.tlkFileLabel.Location = new System.Drawing.Point(25, 173);
            this.tlkFileLabel.Name = "tlkFileLabel";
            this.tlkFileLabel.Size = new System.Drawing.Size(223, 13);
            this.tlkFileLabel.TabIndex = 3;
            this.tlkFileLabel.Text = "Also, select the Infinity Engine TLK file to use";
            // 
            // tlkFileBrowseButton
            // 
            this.tlkFileBrowseButton.Location = new System.Drawing.Point(443, 192);
            this.tlkFileBrowseButton.Name = "tlkFileBrowseButton";
            this.tlkFileBrowseButton.Size = new System.Drawing.Size(75, 23);
            this.tlkFileBrowseButton.TabIndex = 5;
            this.tlkFileBrowseButton.Text = "Browse...";
            this.tlkFileBrowseButton.UseVisualStyleBackColor = true;
            this.tlkFileBrowseButton.Click += new System.EventHandler(this.tlkFileBrowseButton_Click);
            // 
            // dialogFileBrowseButton
            // 
            this.dialogFileBrowseButton.Location = new System.Drawing.Point(443, 109);
            this.dialogFileBrowseButton.Name = "dialogFileBrowseButton";
            this.dialogFileBrowseButton.Size = new System.Drawing.Size(75, 23);
            this.dialogFileBrowseButton.TabIndex = 2;
            this.dialogFileBrowseButton.Text = "Browse...";
            this.dialogFileBrowseButton.UseVisualStyleBackColor = true;
            this.dialogFileBrowseButton.Click += new System.EventHandler(this.dialogFileBrowseButton_Click);
            // 
            // dialogFileNameTextBox
            // 
            this.dialogFileNameTextBox.Location = new System.Drawing.Point(25, 109);
            this.dialogFileNameTextBox.Name = "dialogFileNameTextBox";
            this.dialogFileNameTextBox.Size = new System.Drawing.Size(412, 21);
            this.dialogFileNameTextBox.TabIndex = 1;
            // 
            // convertWizardPage
            // 
            this.convertWizardPage.CaptionTitle = "Conversion";
            this.convertWizardPage.Controls.Add(this.startConversionButton);
            this.convertWizardPage.Controls.Add(this.conversionLogLabel);
            this.convertWizardPage.Controls.Add(this.conversionReportTextBox);
            this.convertWizardPage.FullPage = false;
            this.convertWizardPage.InactiveBackColor = System.Drawing.Color.Empty;
            this.convertWizardPage.InactiveTextBackColor = System.Drawing.Color.Empty;
            this.convertWizardPage.InactiveTextColor = System.Drawing.Color.Empty;
            this.convertWizardPage.Location = new System.Drawing.Point(0, 0);
            this.convertWizardPage.Name = "convertWizardPage";
            this.convertWizardPage.PageDimmed = false;
            this.convertWizardPage.SelectBackColor = System.Drawing.Color.Empty;
            this.convertWizardPage.Selected = false;
            this.convertWizardPage.SelectTextBackColor = System.Drawing.Color.Empty;
            this.convertWizardPage.SelectTextColor = System.Drawing.Color.Empty;
            this.convertWizardPage.Size = new System.Drawing.Size(545, 300);
            this.convertWizardPage.SubTitle = "Click on the \"Start conversion\" button to start";
            this.convertWizardPage.TabIndex = 5;
            this.convertWizardPage.ToolTip = "Page";
            // 
            // startConversionButton
            // 
            this.startConversionButton.Location = new System.Drawing.Point(12, 14);
            this.startConversionButton.Name = "startConversionButton";
            this.startConversionButton.Size = new System.Drawing.Size(99, 23);
            this.startConversionButton.TabIndex = 5;
            this.startConversionButton.Text = "Start conversion";
            this.startConversionButton.UseVisualStyleBackColor = true;
            this.startConversionButton.Click += new System.EventHandler(this.startConversionButton_Click);
            // 
            // conversionLogLabel
            // 
            this.conversionLogLabel.AutoSize = true;
            this.conversionLogLabel.Location = new System.Drawing.Point(9, 54);
            this.conversionLogLabel.Name = "conversionLogLabel";
            this.conversionLogLabel.Size = new System.Drawing.Size(82, 13);
            this.conversionLogLabel.TabIndex = 4;
            this.conversionLogLabel.Text = "Conversion log:";
            // 
            // conversionReportTextBox
            // 
            this.conversionReportTextBox.Location = new System.Drawing.Point(9, 70);
            this.conversionReportTextBox.Multiline = true;
            this.conversionReportTextBox.Name = "conversionReportTextBox";
            this.conversionReportTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.conversionReportTextBox.Size = new System.Drawing.Size(493, 169);
            this.conversionReportTextBox.TabIndex = 3;
            // 
            // dlgFileLabel
            // 
            this.dlgFileLabel.AutoSize = true;
            this.dlgFileLabel.Location = new System.Drawing.Point(25, 93);
            this.dlgFileLabel.Name = "dlgFileLabel";
            this.dlgFileLabel.Size = new System.Drawing.Size(176, 13);
            this.dlgFileLabel.TabIndex = 0;
            this.dlgFileLabel.Text = "Select the .DLG file to convert here";
            // 
            // tlkFileNameComboBox
            // 
            this.tlkFileNameComboBox.FormattingEnabled = true;
            this.tlkFileNameComboBox.Location = new System.Drawing.Point(25, 192);
            this.tlkFileNameComboBox.Name = "tlkFileNameComboBox";
            this.tlkFileNameComboBox.Size = new System.Drawing.Size(412, 21);
            this.tlkFileNameComboBox.TabIndex = 4;
            // 
            // ConversationConverterWizard
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.ClientSize = new System.Drawing.Size(545, 405);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ConversationConverterWizard";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Style = Crownwood.DotNetMagic.Common.VisualStyle.IDE2005;
            this.Text = "Infinity Engine conversation converter wizard";
            this.Load += new System.EventHandler(this.ConversationConverterWizard_Load);
            this.selectFileWizardPage.ResumeLayout(false);
            this.selectFileWizardPage.PerformLayout();
            this.convertWizardPage.ResumeLayout(false);
            this.convertWizardPage.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Crownwood.DotNetMagic.Controls.WizardPage selectFileWizardPage;
        private System.Windows.Forms.Label tlkFileLabel;
        private System.Windows.Forms.Button tlkFileBrowseButton;
        private System.Windows.Forms.Button dialogFileBrowseButton;
        private System.Windows.Forms.TextBox dialogFileNameTextBox;
        private Crownwood.DotNetMagic.Controls.WizardPage convertWizardPage;
        private System.Windows.Forms.Button startConversionButton;
        private System.Windows.Forms.Label conversionLogLabel;
        private System.Windows.Forms.TextBox conversionReportTextBox;
        private System.Windows.Forms.Label dlgFileLabel;
        private System.Windows.Forms.ComboBox tlkFileNameComboBox;

    }
}