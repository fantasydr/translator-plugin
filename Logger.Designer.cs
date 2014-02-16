namespace ConversationTranslator
{
    partial class Logger
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
            this.lstMods = new System.Windows.Forms.ListBox();
            this.splContainer = new System.Windows.Forms.SplitContainer();
            this.cbConversation = new System.Windows.Forms.CheckBox();
            this.cbBlueprint = new System.Windows.Forms.CheckBox();
            this.cbJournal = new System.Windows.Forms.CheckBox();
            this.cbNoTranslate = new System.Windows.Forms.CheckBox();
            this.btnLoad = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.cbReadonly = new System.Windows.Forms.CheckBox();
            this.cbSkipCamp = new System.Windows.Forms.CheckBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.txtLogger = new System.Windows.Forms.TextBox();
            this.splContainer.Panel1.SuspendLayout();
            this.splContainer.Panel2.SuspendLayout();
            this.splContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // lstMods
            // 
            this.lstMods.Dock = System.Windows.Forms.DockStyle.Left;
            this.lstMods.FormattingEnabled = true;
            this.lstMods.ItemHeight = 12;
            this.lstMods.Location = new System.Drawing.Point(0, 0);
            this.lstMods.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.lstMods.Name = "lstMods";
            this.lstMods.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lstMods.Size = new System.Drawing.Size(101, 530);
            this.lstMods.TabIndex = 1;
            // 
            // splContainer
            // 
            this.splContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splContainer.Location = new System.Drawing.Point(101, 0);
            this.splContainer.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.splContainer.Name = "splContainer";
            this.splContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splContainer.Panel1
            // 
            this.splContainer.Panel1.Controls.Add(this.cbConversation);
            this.splContainer.Panel1.Controls.Add(this.cbBlueprint);
            this.splContainer.Panel1.Controls.Add(this.cbJournal);
            this.splContainer.Panel1.Controls.Add(this.cbNoTranslate);
            this.splContainer.Panel1.Controls.Add(this.btnLoad);
            this.splContainer.Panel1.Controls.Add(this.btnSave);
            this.splContainer.Panel1.Controls.Add(this.cbReadonly);
            this.splContainer.Panel1.Controls.Add(this.cbSkipCamp);
            this.splContainer.Panel1.Controls.Add(this.btnStart);
            // 
            // splContainer.Panel2
            // 
            this.splContainer.Panel2.Controls.Add(this.txtLogger);
            this.splContainer.Size = new System.Drawing.Size(712, 530);
            this.splContainer.SplitterDistance = 51;
            this.splContainer.SplitterWidth = 3;
            this.splContainer.TabIndex = 2;
            // 
            // cbConversation
            // 
            this.cbConversation.AutoSize = true;
            this.cbConversation.Checked = true;
            this.cbConversation.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbConversation.Location = new System.Drawing.Point(555, 10);
            this.cbConversation.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.cbConversation.Name = "cbConversation";
            this.cbConversation.Size = new System.Drawing.Size(144, 16);
            this.cbConversation.TabIndex = 7;
            this.cbConversation.Text = "Include Conversation";
            this.cbConversation.UseVisualStyleBackColor = true;
            // 
            // cbBlueprint
            // 
            this.cbBlueprint.AutoSize = true;
            this.cbBlueprint.Checked = true;
            this.cbBlueprint.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbBlueprint.Location = new System.Drawing.Point(427, 10);
            this.cbBlueprint.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.cbBlueprint.Name = "cbBlueprint";
            this.cbBlueprint.Size = new System.Drawing.Size(126, 16);
            this.cbBlueprint.TabIndex = 6;
            this.cbBlueprint.Text = "Include Blueprint";
            this.cbBlueprint.UseVisualStyleBackColor = true;
            // 
            // cbJournal
            // 
            this.cbJournal.AutoSize = true;
            this.cbJournal.Checked = true;
            this.cbJournal.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbJournal.Location = new System.Drawing.Point(310, 10);
            this.cbJournal.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.cbJournal.Name = "cbJournal";
            this.cbJournal.Size = new System.Drawing.Size(114, 16);
            this.cbJournal.TabIndex = 5;
            this.cbJournal.Text = "Include Journal";
            this.cbJournal.UseVisualStyleBackColor = true;
            // 
            // cbNoTranslate
            // 
            this.cbNoTranslate.AutoSize = true;
            this.cbNoTranslate.Checked = true;
            this.cbNoTranslate.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbNoTranslate.Location = new System.Drawing.Point(310, 30);
            this.cbNoTranslate.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.cbNoTranslate.Name = "cbNoTranslate";
            this.cbNoTranslate.Size = new System.Drawing.Size(90, 16);
            this.cbNoTranslate.TabIndex = 3;
            this.cbNoTranslate.Text = "Export Only";
            this.cbNoTranslate.UseVisualStyleBackColor = true;
            // 
            // btnLoad
            // 
            this.btnLoad.Location = new System.Drawing.Point(4, 10);
            this.btnLoad.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(106, 27);
            this.btnLoad.TabIndex = 4;
            this.btnLoad.Text = "Load Setting";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(115, 10);
            this.btnSave.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(105, 27);
            this.btnSave.TabIndex = 3;
            this.btnSave.Text = "Save Setting";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // cbReadonly
            // 
            this.cbReadonly.AutoSize = true;
            this.cbReadonly.Checked = true;
            this.cbReadonly.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbReadonly.Location = new System.Drawing.Point(403, 30);
            this.cbReadonly.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.cbReadonly.Name = "cbReadonly";
            this.cbReadonly.Size = new System.Drawing.Size(78, 16);
            this.cbReadonly.TabIndex = 2;
            this.cbReadonly.Text = "Read Only";
            this.cbReadonly.UseVisualStyleBackColor = true;
            // 
            // cbSkipCamp
            // 
            this.cbSkipCamp.AutoSize = true;
            this.cbSkipCamp.Location = new System.Drawing.Point(483, 30);
            this.cbSkipCamp.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.cbSkipCamp.Name = "cbSkipCamp";
            this.cbSkipCamp.Size = new System.Drawing.Size(102, 16);
            this.cbSkipCamp.TabIndex = 1;
            this.cbSkipCamp.Text = "Skip Campaign";
            this.cbSkipCamp.UseVisualStyleBackColor = true;
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(224, 10);
            this.btnStart.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(82, 27);
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // txtLogger
            // 
            this.txtLogger.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtLogger.Location = new System.Drawing.Point(0, 0);
            this.txtLogger.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtLogger.Multiline = true;
            this.txtLogger.Name = "txtLogger";
            this.txtLogger.ReadOnly = true;
            this.txtLogger.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtLogger.Size = new System.Drawing.Size(712, 476);
            this.txtLogger.TabIndex = 1;
            // 
            // Logger
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(813, 530);
            this.Controls.Add(this.splContainer);
            this.Controls.Add(this.lstMods);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "Logger";
            this.Text = "Conversation Translator";
            this.Load += new System.EventHandler(this.Logger_Load);
            this.splContainer.Panel1.ResumeLayout(false);
            this.splContainer.Panel1.PerformLayout();
            this.splContainer.Panel2.ResumeLayout(false);
            this.splContainer.Panel2.PerformLayout();
            this.splContainer.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox lstMods;
        private System.Windows.Forms.SplitContainer splContainer;
        private System.Windows.Forms.TextBox txtLogger;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.CheckBox cbReadonly;
        private System.Windows.Forms.CheckBox cbSkipCamp;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.CheckBox cbNoTranslate;
        private System.Windows.Forms.CheckBox cbJournal;
        private System.Windows.Forms.CheckBox cbBlueprint;
        private System.Windows.Forms.CheckBox cbConversation;
    }
}