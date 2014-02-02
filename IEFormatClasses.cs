using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace IEConversationConvert
{
    class IEState
    {
        string text;

        List<IETransition> transitions;
        string trigger;
        int triggerIndex;

        public string Text
        {
            get { return text; }
        }
        public List<IETransition> Transitions
        {
            get { return transitions; }
        }
        public string Trigger
        {
            get { return trigger; }
        }
        public int TriggerIndex
        {
            get { return triggerIndex; }
        }

        public void Read(Stream stream, IETalk tlk, List<IETransition> transitionTriggerTable, IEAITable stateTriggerTable)
        {
            BinaryReader r = new BinaryReader(stream);
            text = tlk[r.ReadInt32()];

            int firstTransitionIndex = r.ReadInt32();
            int transitionCount = r.ReadInt32();

            transitions = new List<IETransition>();
            for (int transitionTriggerIndex = firstTransitionIndex; transitionTriggerIndex < firstTransitionIndex + transitionCount; transitionTriggerIndex++)
            {
                transitions.Add(transitionTriggerTable[transitionTriggerIndex]);
            }

            triggerIndex = r.ReadInt32();
            if (triggerIndex != -1)
            {
                trigger = stateTriggerTable[triggerIndex];
            }
            else
            {
                trigger = null;
            }
        }
    }

    [Flags]
    enum IETransitionFlags
    {
        HasText = 1,
        HasTrigger = 2,
        HasAction = 4,
        TeminatesDialog = 8,
        HasJournalEntry = 16
    }

    class IETransition
    {
        IETransitionFlags flags;
        string text;
        string journalText;
        string transitionTrigger;
        string action;
        string nextDlgResRef;
        int nextStateIndex;
        IEState nextState;

        public string Text
        {
            get { return text; }
        }
        public string JournalText
        {
            get { return journalText; }
        }
        public string TransitionTrigger
        {
            get { return transitionTrigger; }
        }
        public string Action
        {
            get { return action; }
        }
        public string NextDlgResRef
        {
            get { return nextDlgResRef; }
        }
        public int NextStateIndex
        {
            get { return nextStateIndex; }
        }
        public IEState NextState
        {
            get { return nextState; }
            set { nextState = value; }
        }

        public void Read(Stream stream, IETalk tlk, IEAITable transitionTriggerTable, IEAITable actionTable)
        {
            BinaryReader r = new BinaryReader(stream);
            flags = (IETransitionFlags)r.ReadInt32();
            if ((flags & IETransitionFlags.HasText) != 0)
            {
                text = tlk[r.ReadInt32()];
            }
            else
            {
                r.ReadInt32();
                text = null;
            }
            if ((flags & IETransitionFlags.HasJournalEntry) != 0)
            {
                journalText = tlk[r.ReadInt32()];
            }
            else
            {
                r.ReadInt32();
                journalText = null;
            }
            if ((flags & IETransitionFlags.HasTrigger) != 0)
            {
                transitionTrigger = transitionTriggerTable[r.ReadInt32()];
            }
            else
            {
                r.ReadInt32();
                transitionTrigger = null;
            }
            if ((flags & IETransitionFlags.HasAction) != 0)
            {
                action = actionTable[r.ReadInt32()];
            }
            else
            {
                r.ReadInt32();
                action = null;
            }
            if ((flags & IETransitionFlags.TeminatesDialog) == 0)
            {
                nextDlgResRef = new string(r.ReadChars(8)).TrimEnd('\0');
                nextStateIndex = r.ReadInt32();
            }
            else
            {
                r.ReadChars(8);
                r.ReadInt32();
                nextStateIndex = 0;
                nextDlgResRef = null;
            }
        }
    }

    class IEAITable : Dictionary<int, string>
    {
        public void Read(Stream stream, int offset, int count)
        {
            BinaryReader r = new BinaryReader(stream);

            r.BaseStream.Seek(offset, SeekOrigin.Begin);
            for (int index = 0; index < count; index++)
            {
                int stringOffset = r.ReadInt32();
                int stringLength = r.ReadInt32();

                long pos = r.BaseStream.Position;
                r.BaseStream.Seek(stringOffset, SeekOrigin.Begin);
                char[] text = r.ReadChars(stringLength);
                this.Add(index, new string(text));
                r.BaseStream.Seek(pos, SeekOrigin.Begin);
            }
        }
    }

    class IEDialog
    {
        List<IEState> states;
        List<IETransition> transitions;
        string resref;

        public List<IEState> States
        {
            get { return states; }
        }
        public List<IETransition> Transitions
        {
            get { return transitions; }
        }
        public string Resref
        {
            get { return resref; }
        }
        public List<IEState> StartStates
        {
            get
            {
                List<IEState> startStates = new List<IEState>();
                foreach (IEState state in States)
                {
                    if (!(state.Trigger == null) && !state.Trigger.ToLower().StartsWith("false()"))
                    {
                        startStates.Add(state);
                    }
                }
                startStates.Sort(delegate(IEState state1, IEState state2) { return state1.TriggerIndex.CompareTo(state2.TriggerIndex); });
                return startStates;
            }
        }

        public void Read(string filename, IETalk tlk)
        {
            using (Stream stream = File.OpenRead(filename))
            {
                this.Read(stream, tlk);
                stream.Close();
                resref = Path.GetFileNameWithoutExtension(filename);
            }
        }

        public void Read(Stream stream, IETalk tlk)
        {
            int statesCount;
            int statesOffset;
            int transitionsCount;
            int transitionsOffset;
            int stateTriggerOffset;
            int stateTriggerCount;
            int transitionTriggerOffset;
            int transitionTriggerCount;
            int actionsOffset;
            int actionsCount;

            BinaryReader r = new BinaryReader(stream);
            string signature = new string(r.ReadChars(4));
            if (signature != "DLG ")
            {
                throw new ApplicationException("Invalid file format");
            }
            string version = new string(r.ReadChars(4));
            if (version != "V1.0")
            {
                throw new ApplicationException("Invalid file format version");
            }

            statesCount = r.ReadInt32();
            statesOffset = r.ReadInt32();
            transitionsCount = r.ReadInt32();
            transitionsOffset = r.ReadInt32();
            stateTriggerOffset = r.ReadInt32();
            stateTriggerCount = r.ReadInt32();
            transitionTriggerOffset = r.ReadInt32();
            transitionTriggerCount = r.ReadInt32();
            actionsOffset = r.ReadInt32();
            actionsCount = r.ReadInt32();

            IEAITable stateTriggerTable = new IEAITable();
            stateTriggerTable.Read(r.BaseStream, stateTriggerOffset, stateTriggerCount);

            IEAITable transitionTriggerTable = new IEAITable();
            transitionTriggerTable.Read(r.BaseStream, transitionTriggerOffset, transitionTriggerCount);

            IEAITable actionTable = new IEAITable();
            actionTable.Read(r.BaseStream, actionsOffset, actionsCount);

            transitions = new List<IETransition>();
            r.BaseStream.Seek(transitionsOffset, SeekOrigin.Begin);
            for (int transitionIndex = 0; transitionIndex < transitionsCount; transitionIndex++)
            {
                IETransition transition = new IETransition();
                transition.Read(r.BaseStream, tlk, transitionTriggerTable, actionTable);
                transitions.Add(transition);
            }

            states = new List<IEState>();
            r.BaseStream.Seek(statesOffset, SeekOrigin.Begin);
            for (int stateIndex = 0; stateIndex < statesCount; stateIndex++)
            {
                IEState state = new IEState();
                state.Read(r.BaseStream, tlk, transitions, stateTriggerTable);
                states.Add(state);
            }
        }
        public void LinkTransitionStates(List<IEState> states, string resref)
        {
            foreach (IETransition transition in this.Transitions)
            {
                if (string.Compare(transition.NextDlgResRef, resref, true) == 0)
                {
                    transition.NextState = states[transition.NextStateIndex];
                }
            }
        }
    }

    class IETalk : Dictionary<int, string>
    {
        public void Read(string filename)
        {
            using (Stream stream = File.OpenRead(filename))
            {
                this.Read(stream);
                stream.Close();
            }
        }

        public void Read(Stream stream)
        {
            int stringsOffset;
            int stringsCount;

            BinaryReader r = new BinaryReader(stream, System.Text.Encoding.Default);
            string signature = new string(r.ReadChars(4));
            if (signature != "TLK ")
            {
                throw new ApplicationException("Invalid file format");
            }
            string version = new string(r.ReadChars(4));
            if (version != "V1  ")
            {
                throw new ApplicationException("Invalid file format version");
            }
            r.ReadInt16();
            stringsCount = r.ReadInt32();
            stringsOffset = r.ReadInt32();

            for (int stringIndex = 0; stringIndex < stringsCount; stringIndex++)
            {
                short flag = r.ReadInt16();
                r.ReadChars(8); // Sound
                int volume = r.ReadInt32();
                int pitch = r.ReadInt32();
                int stringOffset = r.ReadInt32();
                int stringLength = r.ReadInt32();

                long pos = r.BaseStream.Position;
                r.BaseStream.Seek(stringsOffset + stringOffset, SeekOrigin.Begin);
                char[] text = r.ReadChars(stringLength);
                this.Add(stringIndex, new string(text));
                r.BaseStream.Seek(pos, SeekOrigin.Begin);
            }
        }
    }
    class IEDialogCollection : List<IEDialog>
    {
        public string[] GetNeededFiles()
        {
            List<string> neededFiles = new List<string>();
            foreach (IEDialog dlg in this)
            {
                foreach (IETransition transition in dlg.Transitions)
                {
                    if (transition.NextDlgResRef != null)
                    {
                        if (!this.Exists(delegate(IEDialog dlgMatch) { return string.Compare(dlgMatch.Resref, transition.NextDlgResRef,true) == 0; }))
                        {
                            if (!neededFiles.Contains(transition.NextDlgResRef))
                            {
                                neededFiles.Add(transition.NextDlgResRef);
                            }
                        }
                    }
                }
            }
            return neededFiles.ToArray();
        }

        public void LinkAllToAll()
        {
            foreach (IEDialog dlg in this)
            {
                foreach (IEDialog dlg2 in this)
                {
                    dlg.LinkTransitionStates(dlg2.States, dlg2.Resref);
                }
            }
        }
    }
}
