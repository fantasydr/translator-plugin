using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace FDRConversationTranslator
{
    public partial class Logger : Form, ILogger
    {
        Translator _trans;
        public Logger()
        {
            InitializeComponent();
        }

        Setting _settings = new Setting();

        Setting DefaultSettings()
        {
            // sample settings
            Setting newSetting = new Setting();
            newSetting.origin = @"C:\origin.txt";
            newSetting.target = @"C:\target.txt";
            newSetting.root = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), @"Neverwinter Nights 2\modules\");
            newSetting.miss = @"C:\miss.log";
            newSetting.output = @"C:\output.log";

            newSetting.modules = new string[]{
                                "Module1",
                                "Module2",
            };

            return newSetting;
        }

        void LoadSettings(string filename)
        {
            filename = filename.Trim();

            if (filename.Length > 0)
            {
                AppendLog(string.Format("Loading setting file {0}...", filename));

                using (StreamReader sr = new StreamReader(filename))
                {
                    XmlSerializer s = new XmlSerializer(_settings.GetType());
                    _settings = s.Deserialize(sr) as Setting;
                }
            }
            else
            {
                _settings = DefaultSettings();
            }

            AppendLog(string.Format("Setting loaded:\r\n    Origin:{0}\r\n    Target:{1}\r\n    Miss:{2}\r\n    Output:{3}\r\n    Root:{4}\r\n", 
                                                           _settings.origin, _settings.target,
                                                           _settings.miss, _settings.output,
                                                           _settings.root));

            lstMods.Items.Clear();
            foreach (var s in _settings.modules)
                lstMods.Items.Add(s);
        }

        void SaveSetting(string filename)
        {
            AppendLog(string.Format("Saving setting file {0}...", filename));

            using (StreamWriter sw = new StreamWriter(filename))
            {
                XmlSerializer s = new XmlSerializer(_settings.GetType());
                s.Serialize(sw, _settings);
            }
        }

        int _counter = 0;
        public void AppendLog(string content)
        {
            this.BeginInvoke(new System.Threading.ThreadStart(delegate()
            {
                _counter++;
                if (_counter > 1000)
                {
                    txtLogger.Clear();
                    _counter = 0;
                }
                txtLogger.AppendText(content + "\r\n");
            }));

            Application.DoEvents();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            List<string> selected = new List<string>();

            StringBuilder sb = new StringBuilder();

            sb.AppendLine(string.Format("Continue?  (ReadOnly: {0})", cbReadonly.Checked));
            sb.AppendLine("These Modules will be exported: ");
            foreach (var s in lstMods.SelectedItems)
            {
                selected.Add((string)s);
                if (selected.Count < 30)
                {
                    sb.AppendLine((string)s);
                }
                else if (selected.Count == 50)
                {
                    sb.AppendLine("...");
                }
            }
            sb.AppendLine(string.Format("Total: {0}", selected.Count));

            if (selected.Count > 0)
            {
                if (MessageBox.Show(sb.ToString(), "Question", MessageBoxButtons.OKCancel) != DialogResult.OK)
                    return;

                _trans = new Translator(_settings.origin,
                                              _settings.target,
                                              this, cbReadonly.Checked);

                _trans.ConvertConversation(_settings.root,
                                           selected.ToArray(),
                                           cbSkipCamp.Checked,
                                           _settings.miss,
                                           _settings.output);

                MessageBox.Show("Finished!");
            }
            else
            {
                MessageBox.Show("Did nothing...");
            }
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog fd = new OpenFileDialog())
            {
                fd.Filter = "Setting files (*.xml)|*.xml|All files (*.*)|*.*";
                if (fd.ShowDialog() == DialogResult.OK)
                {
                    LoadSettings(fd.FileName);
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog fd = new SaveFileDialog())
            {
                fd.Filter = "Setting files (*.xml)|*.xml|All files (*.*)|*.*";
                if (fd.ShowDialog() == DialogResult.OK)
                {
                    SaveSetting(fd.FileName);
                }
            }
        }

        private void Logger_Load(object sender, EventArgs e)
        {
            AppendLog("================================");
            AppendLog(" If you want to change the setting, save it and modify the XML.");
            AppendLog(" Then load the modified XML again.");
            AppendLog("================================");

            LoadSettings("");
        }
    }

    public class Setting
    {
        public string[] modules; // valid modules
        public string root; // module root folder (<game foder>/modules)

        public string origin; // origin text (E.X: english version)
        public string target; // target text (E.X: chinese version)

        public string miss; // missing record
        public string output; // output log
    }
}
