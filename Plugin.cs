using System;
using System.Windows.Forms;
using TD.SandBar;
using NWN2Toolset;
using NWN2Toolset.Plugins;

namespace FDRConversationTranslator
{
    public class ConversationTranslatorPlugin : INWN2Plugin
    {
        private MenuButtonItem _menuItem;

        private void HandlePluginLaunch(object sender, EventArgs e)
        {
            using (Logger frm = new Logger())
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
            _menuItem = cHost.GetMenuForPlugin(this);
            _menuItem.Activate += new EventHandler(this.HandlePluginLaunch);
        }

        public void Unload(INWN2PluginHost cHost)
        {
        }

        public MenuButtonItem PluginMenuItem
        {
            get
            {
                return _menuItem;
            }
        }

        public string DisplayName
        {
            get
            {
                return "Conversation Translator";
            }
        }

        public string MenuName
        {
            get
            {
                return "Conversation Translator";
            }
        }

        public string Name
        {
            get
            {
                return "FDRConversationTranslator";
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
