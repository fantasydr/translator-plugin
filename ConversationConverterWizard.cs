using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.IO;
using System.Windows.Forms;
using Microsoft.Win32;

namespace IEConversationConvert
{
    public partial class ConversationConverterWizard : Crownwood.DotNetMagic.Forms.WizardDialog
    {
        public ConversationConverterWizard()
        {
            InitializeComponent();
        }

        private IEDialogCollection dialogs = null;

        private void dialogFileBrowseButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = false;
            openFileDialog.ShowReadOnly = false;
            openFileDialog.Filter = "Dialog files (*.dlg)|*.dlg|All files (*.*)|*.*";
            openFileDialog.DefaultExt = "dlg";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                dialogFileNameTextBox.Text = openFileDialog.FileName;
            }
        }

        private void tlkFileBrowseButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = false;
            openFileDialog.ShowReadOnly = false;
            openFileDialog.Filter = "TLK files (*.tlk)|*.tlk|All files (*.*)|*.*";
            openFileDialog.DefaultExt = "tlk";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                tlkFileNameComboBox.Text = openFileDialog.FileName;
            }
        }

        private void Log(string text)
        {
            conversionReportTextBox.Text += text + "\r\n";
        }

        private void WizardControlField_NextClick(object sender, CancelEventArgs e)
        {
            if (WizardControlProperty.SelectedIndex == 0)
            {
                if (!File.Exists(dialogFileNameTextBox.Text))
                {
                    MessageBox.Show("Please select a dialog file");
                    e.Cancel = true;
                    return;
                }
                if (!File.Exists(tlkFileNameComboBox.Text))
                {
                    MessageBox.Show("Please select a tlk file");
                    e.Cancel = true;
                    return;
                }
                IETalk tlk = new IETalk();
                try
                {
                    tlk.Read(tlkFileNameComboBox.Text);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error while reading TLK file:" + Environment.NewLine + ex.ToString());
                    e.Cancel = true;
                    return;
                }

                IEDialog dlg = new IEDialog();
                try
                {
                    dlg.Read(dialogFileNameTextBox.Text, tlk);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error while reading dialog file:" + Environment.NewLine + ex.ToString());
                    e.Cancel = true;
                    return;
                }

                dialogs = new IEDialogCollection();
                dialogs.Add(dlg);

                string[] neededFiles;
                bool fileNotFound = false;
                do
                {
                    neededFiles = dialogs.GetNeededFiles();
                    if (neededFiles.Length == 0) break;
                    foreach (string neededFile in neededFiles)
                    {
                        string neededFileName = Path.Combine(Path.GetDirectoryName(dialogFileNameTextBox.Text), neededFile + ".dlg");
                        if (File.Exists(neededFileName))
                        {
                            IEDialog neededDlg = new IEDialog();
                            try
                            {
                                neededDlg.Read(neededFileName, tlk);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Error while reading dependent dialog file " + neededFileName + ":" + Environment.NewLine + ex.ToString());
                                e.Cancel = true;
                                return;
                            }
                            dialogs.Add(neededDlg);
                        }
                        else
                        {
                            fileNotFound = true;
                        }
                    }
                } while (!fileNotFound);
                neededFiles = dialogs.GetNeededFiles();
                if (neededFiles.Length > 0)
                {
                    MessageBox.Show("This dialog is dependent on these files" + Environment.NewLine + string.Join(Environment.NewLine, neededFiles) + Environment.NewLine + Environment.NewLine + "Place these files in the same folder as the primary dialog file.");
                    e.Cancel = true;
                }
            }
        }

        private void startConversionButton_Click(object sender, EventArgs e)
        {
            dialogs.LinkAllToAll();

            try
            {
                IEConversationConvert convert = new IEConversationConvert();
                convert.DoConvert(dialogs);
                Log("Successfully converted " + dialogs[0].Resref);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while converting:" + Environment.NewLine + ex.ToString());
            }
        }

        private void ConversationConverterWizard_Load(object sender, EventArgs e)
        {
            // Try to read Infinity Explorer's Recent Games
            RegistryKey infExpRecentGamesKey = Registry.CurrentUser.OpenSubKey(@"Software\Yoletir\InfExp\Recent Games");
            if (infExpRecentGamesKey != null)
            {
                int count = (int)infExpRecentGamesKey.GetValue("Count", 0);
                for (int gameNr = 0; gameNr < count; gameNr++)
                {
                    string gameFolder = (string)infExpRecentGamesKey.GetValue("Game" + gameNr);
                    if (gameFolder != null)
                    {
                        tlkFileNameComboBox.Items.Add(Path.Combine(gameFolder, "dialog.tlk"));
                    }
                }
                if (tlkFileNameComboBox.Items.Count > 0)
                {
                    tlkFileNameComboBox.SelectedIndex = 0;
                }
            }
            infExpRecentGamesKey.Close();
        }
    }
}
