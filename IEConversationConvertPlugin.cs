using System;
using System.Windows.Forms;
using TD.SandBar;
using NWN2Toolset;
using NWN2Toolset.Plugins;

namespace IEConversationConvert
{
    public class IEConversationConvertPlugin : INWN2Plugin
    {
        private MenuButtonItem m_cMenuItem;

        private void HandlePluginLaunch(object sender, EventArgs e)
        {
            using (ConversationConverterWizard frm = new ConversationConverterWizard())
            {
                frm.ShowDialog();
            }
        }

        public void Load(INWN2PluginHost cHost)
        {
        }

        public void Shutdown(INWN2PluginHost cHost)
        {
        }

        public void Startup(INWN2PluginHost cHost)
        {
            m_cMenuItem = cHost.GetMenuForPlugin(this);
            m_cMenuItem.Activate += new EventHandler(this.HandlePluginLaunch);
        }

        public void Unload(INWN2PluginHost cHost)
        {
        }

        public MenuButtonItem PluginMenuItem
        {
            get
            {
                return m_cMenuItem;
            }
        }

        public string DisplayName
        {
            get
            {
                return "Infinity Engine Conversation Converter";
            }
        }

        public string MenuName
        {
            get
            {
                return "Infinity Engine Conversation Converter";
            }
        }

        public string Name
        {
            get
            {
                return "IEConversationConvert";
            }
        }

        public object Preferences
        {
            get
            {
                return null;
            }
            set
            {
            }
        }

    }
}
