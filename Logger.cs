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
        void LoadSettings(string filename)
        {
            AppendLog(string.Format("Loading setting file {0}...", filename));

            Setting newSetting = null;

            using (StreamReader sr = new StreamReader(filename))
            {
                XmlSerializer s = new XmlSerializer(_settings.GetType());
                newSetting = s.Deserialize(sr) as Setting;
            }

            //// sample settings
            //newSetting = new Setting();
            //newSetting.origin = @"D:\InfinityMod\BGEE\decompiled\en_US-11-08\string-en_US.txt";
            //newSetting.target = @"D:\InfinityMod\BGEE\decompiled\zh_CN-11-25\string-zh_CN.txt";
            //newSetting.root = @"D:\Documents\Neverwinter Nights 2\modules\";
            //newSetting.miss = @"D:\miss.log";
            //newSetting.output = @"D:\output.log";
            //newSetting.modules = new string[]{
            //                    "BG_A",
            //                    "BG_B1",
            //                    "BG_B2",
            //                    "BG_C",
            //                    "BG_C1",
            //                    "BG_D1",
            //                    "BG_D2",
            //                    "BG_D3",
            //                    "BG_D4",
            //                    "BG_E",
            //                    "BG_F",
            //                    "BG_G",
            //                    "BG_H",
            //                    "BG_I",
            //                    "BG_J",
            //                    "BG_LOBBY",
            //                    "TOSC_A",
            //                    "TOSC_B",
            //                    "TOSC_C",
            //                    "TOSC_D.mod",
            //                    "TOSC_E.mod",
            //};

            //
            _settings = newSetting;

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
