﻿namespace FDRConversationTranslator
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
            this.lstMods.ItemHeight = 15;
            this.lstMods.Location = new System.Drawing.Point(0, 0);
            this.lstMods.Name = "lstMods";
            this.lstMods.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lstMods.Size = new System.Drawing.Size(133, 663);
            this.lstMods.TabIndex = 1;
            // 
            // splContainer
            // 
            this.splContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splContainer.Location = new System.Drawing.Point(133, 0);
            this.splContainer.Name = "splContainer";
            this.splContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splContainer.Panel1
            // 
            this.splContainer.Panel1.Controls.Add(this.btnLoad);
            this.splContainer.Panel1.Controls.Add(this.btnSave);
            this.splContainer.Panel1.Controls.Add(this.cbReadonly);
            this.splContainer.Panel1.Controls.Add(this.cbSkipCamp);
            this.splContainer.Panel1.Controls.Add(this.btnStart);
            // 
            // splContainer.Panel2
            // 
            this.splContainer.Panel2.Controls.Add(this.txtLogger);
            this.splContainer.Size = new System.Drawing.Size(951, 663);
            this.splContainer.SplitterDistance = 65;
            this.splContainer.TabIndex = 2;
            // 
            // btnLoad
            // 
            this.btnLoad.Location = new System.Drawing.Point(6, 12);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(141, 34);
            this.btnLoad.TabIndex = 4;
            this.btnLoad.Text = "Load Setting";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(153, 12);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(140, 34);
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
            this.cbReadonly.Location = new System.Drawing.Point(806, 12);
            this.cbReadonly.Name = "cbReadonly";
            this.cbReadonly.Size = new System.Drawing.Size(93, 19);
            this.cbReadonly.TabIndex = 2;
            this.cbReadonly.Text = "ReadOnly";
            this.cbReadonly.UseVisualStyleBackColor = true;
            // 
            // cbSkipCamp
            // 
            this.cbSkipCamp.AutoSize = true;
            this.cbSkipCamp.Checked = true;
            this.cbSkipCamp.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbSkipCamp.Location = new System.Drawing.Point(806, 37);
            this.cbSkipCamp.Name = "cbSkipCamp";
            this.cbSkipCamp.Size = new System.Drawing.Size(133, 19);
            this.cbSkipCamp.TabIndex = 1;
            this.cbSkipCamp.Text = "Skip Campaign";
            this.cbSkipCamp.UseVisualStyleBackColor = true;
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(691, 12);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(109, 34);
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // txtLogger
            // 
            this.txtLogger.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtLogger.Location = new System.Drawing.Point(0, 0);
            this.txtLogger.Multiline = true;
            this.txtLogger.Name = "txtLogger";
            this.txtLogger.ReadOnly = true;
            this.txtLogger.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtLogger.Size = new System.Drawing.Size(951, 594);
            this.txtLogger.TabIndex = 1;
            // 
            // Logger
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1084, 663);
            this.Controls.Add(this.splContainer);
            this.Controls.Add(this.lstMods);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Logger";
            this.Text = "Conversation Translator";
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
    }
}