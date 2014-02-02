using System;
using System.Collections.Generic;
using System.IO;
using NWN2Toolset.NWN2.Data;
using NWN2Toolset.NWN2.Data.ConversationData;

namespace IEConversationConvert
{
    class IEConversationConvert
    {
        public void DoConvert(IEDialogCollection dialogs)
        {
            NWN2GameConversation conv;
            conv = new NWN2GameConversation(dialogs[0].Resref, NWN2Toolset.NWN2ToolsetMainForm.App.Module.TempDirectory, NWN2Toolset.NWN2ToolsetMainForm.App.Module.Repositories[0]);
            NWN2Toolset.NWN2ToolsetMainForm.App.Module.Conversations.Add(conv);
            conv.Demand();

            Dictionary<IEState, NWN2ConversationLine> mapStates = new Dictionary<IEState, NWN2ConversationLine>();
            Dictionary<IETransition, NWN2ConversationLine> mapTransitions = new Dictionary<IETransition, NWN2ConversationLine>();

            foreach (IEState state in dialogs[0].StartStates)
            {
                NWN2ConversationConnector connector = new NWN2ConversationConnector();
                connector.ConnectorID.ConversationHash = conv.GetDataHashCode();
                conv.ConversationData.m_cAllConnectors.Add(connector);
                connector.Type = NWN2ConversationConnectorType.StartingEntry;
                NWN2ConversationLine line = ConvertState(conv, state, mapStates, mapTransitions, connector);

                conv.StartingList.Add(connector);
            }

            conv.HasChanged = true;

            conv.OEISerialize(conv.Resource.GetStream(true));
            conv.Release();
        }

        private NWN2ConversationLine ConvertState(NWN2GameConversation conv, IEState state, Dictionary<IEState, NWN2ConversationLine> mapStates, Dictionary<IETransition, NWN2ConversationLine> mapTransitions, NWN2ConversationConnector owningConnector)
        {
            NWN2ConversationLine line = null;
            if (mapStates.ContainsKey(state))
            {
                line = mapStates[state];
            }
            if (line == null)
            {
                line = new NWN2ConversationLine();
                line.Text = ConvertText(state.Text);
                line.OwningConnector = owningConnector;
                line.Entry = true;
                owningConnector.Line = line;
                conv.Entries.Add(line);
                mapStates[state] = line;
            }
            else
            {
                owningConnector.Line = line;
                owningConnector.Link = true;
                return line;
            }
            foreach (IETransition transition in state.Transitions)
            {
                NWN2ConversationConnector connector = new NWN2ConversationConnector();
                connector.ConnectorID.ConversationHash = conv.GetDataHashCode();
                conv.ConversationData.m_cAllConnectors.Add(connector);
                connector.Type = NWN2ConversationConnectorType.Reply;
                line.Children.Add(connector);
                
                NWN2ConversationLine childLine = null;
                if (mapTransitions.ContainsKey(transition))
                {
                    childLine = mapTransitions[transition];
                }
                if (childLine == null)
                {
                    childLine = new NWN2ConversationLine();
                    childLine.OwningConnector = connector;
                    if (!string.IsNullOrEmpty(transition.Text))
                    {
                        childLine.Text = ConvertText(transition.Text);
                    }
                    childLine.Entry = false;
                    conv.Replies.Add(childLine);
                    mapTransitions[transition] = childLine;

                    if (transition.NextState != null)
                    {
                        NWN2ConversationConnector connector2 = new NWN2ConversationConnector();
                        connector2.ConnectorID.ConversationHash = conv.GetDataHashCode();
                        conv.ConversationData.m_cAllConnectors.Add(connector2);
                        connector2.Type = NWN2ConversationConnectorType.Entry;

                        NWN2ConversationLine nextStateLine = ConvertState(conv, transition.NextState, mapStates, mapTransitions, connector2);

                        childLine.Children.Add(connector2);
                    }
                }
                else
                {
                    connector.Link = true;
                }
                connector.Line = childLine;
            }
            return line;
        }

        private OEIShared.Utils.OEIExoLocString ConvertText(string text)
        {
            // Usable IE tokens taken from http://iesdp.gibberlings3.net/main.htm

            text = text.Replace("<BROTHERSISTER>", "<brother/sister>");
            text = text.Replace("<CHARNAME>", "<FullName>");
            //text = text.Replace("<DAY>", "");
            //text = text.Replace("<DAYANDMONTH>", "");
            text = text.Replace("<DAYNIGHT>", "<day/night>");
            text = text.Replace("<DAYNIGHTALL>", "<quarterday>");
            //text = text.Replace("<DURATION>", "");
            //text = text.Replace("<DURATIONNOAND>", "");
            text = text.Replace("<GABBER>", "<FullName>");
            //text = text.Replace("<GAMEDAY>", "");
            //text = text.Replace("<GAMEDAYS>", "");
            text = text.Replace("<GIRLBOY>", "<boy/girl>");
            text = text.Replace("<HESHE>", "<he/she>");
            text = text.Replace("<HIMHER>", "<him/her>");
            text = text.Replace("<HISHER>", "<his/her>");
            //text = text.Replace("<HOUR>", "");
            text = text.Replace("<LADYLORD>", "<lord/lady>");
            text = text.Replace("<LEVEL>", "<Level>");
            text = text.Replace("<MALEFEMALE>", "<male/female>");
            text = text.Replace("<MANWOMAN>", "<man/woman>");
            //text = text.Replace("<MINUTE>", "");
            text = text.Replace("<MONTH>", "<GameMonth>");
            //text = text.Replace("<MONTHNAME>", "");
            //text = text.Replace("<PLAYER1-6>", "");
            text = text.Replace("<PRO_BROTHERSISTER>", "<brother/sister>");
            text = text.Replace("<PRO_GIRLBOY>", "<boy/girl>");
            text = text.Replace("<PRO_HESHE>", "<he/she>");
            text = text.Replace("<PRO_HIMHER>", "<him/her>");
            text = text.Replace("<PRO_HISHER>", "<his/her>");
            text = text.Replace("<PRO_LADYLORD>", "<lord/lady>");
            text = text.Replace("<PRO_MALEFEMALE>", "<male/female>");
            text = text.Replace("<PRO_MANWOMAN>", "<man/woman>");
            text = text.Replace("<PRO_RACE>", "<race>");
            text = text.Replace("<PRO_SIRMAAM>", "<sir/madam>");
            text = text.Replace("<PROTAGONIST_BROTHERSISTER>", "<brother/sister>");
            text = text.Replace("<PROTAGONIST_GIRLBO>", "<boy/girl>");
            text = text.Replace("<PROTAGONIST_HESHE>", "<he/she>");
            text = text.Replace("<PROTAGONIST_HIMHER>", "<him/her>");
            text = text.Replace("<PROTAGONIST_HISHER>", "<his/her>");
            text = text.Replace("<PROTAGONIST_LADYLORD>", "<lord/lady>");
            text = text.Replace("<PROTAGONIST_MALEFEMALE>", "<male/female>");
            text = text.Replace("<PROTAGONIST_MANWOMAN>", "<man/woman>");
            text = text.Replace("<PROTAGONIST_RACE>", "<race>");
            text = text.Replace("<PROTAGONIST_SIRMAAM>", "<sir/madam>");
            //text = text.Replace("<PRO_SONDAUGHTER>", "");
            text = text.Replace("<RACE>", "<race>");
            text = text.Replace("<SIRMAAM>", "<sir/madam>");
            //text = text.Replace("<SONDAUGHTER>", "");
            text = text.Replace("<TM>", "™");
            text = text.Replace("<YEAR>", "<GameYear>");

            OEIShared.Utils.OEIExoLocString oeiString = new OEIShared.Utils.OEIExoLocString();
            oeiString.SetString(text, OEIShared.Utils.BWLanguages.BWLanguage.English, OEIShared.Utils.BWLanguages.Gender.Male);
            return oeiString;
        }

    }
}
