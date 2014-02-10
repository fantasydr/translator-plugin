using NWN2Toolset.NWN2.Data;
using NWN2Toolset.NWN2.Data.Blueprints;
using NWN2Toolset.NWN2.Data.ConversationData;
using NWN2Toolset.NWN2.Data.Journal;
using NWN2Toolset.NWN2.Data.Templates;
using NWN2Toolset.NWN2.Data.TypedCollections;
using OEIShared.IO;
using OEIShared.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace ConversationTranslator
{
    #region String Matching
    public class LevenshteinDistance
    {
        private int LowerOfThree(int first, int second, int third)
        {
            int min = Math.Min(first, second);
            return Math.Min(min, third);
        }

        int[] Matrix;

        int GetMatrix(int x, int y, int stride)
        {
            return Matrix[x + y * stride];
        }

        void SetMatrix(int x, int y, int stride, int value)
        {
            Matrix[x + y * stride] = value;
        }

        public int Calc(string str1, string str2, int early)
        {
            int n = str1.Length;
            int m = str2.Length;

            int temp = 0;
            char ch1;
            char ch2;
            int i = 0;
            int j = 0;
            if (n == 0)
            {
                return m;
            }
            if (m == 0)
            {

                return n;
            }

            // do not alloc every time
            int stride = n + 1;
            int height = m + 1;
            if (Matrix == null || Matrix.Length < stride * height)
            {
                Matrix = new int[stride * height];
            }

            for (i = 0; i <= n; i++)
            {
                //初始化第一列
                SetMatrix(i, 0, stride, i);
            }

            for (j = 0; j <= m; j++)
            {
                //初始化第一行
                SetMatrix(0, j, stride, j);
            }

            for (i = 1; i <= n; i++)
            {
                ch1 = str1[i - 1];
                for (j = 1; j <= m; j++)
                {
                    ch2 = str2[j - 1];
                    if (ch1.Equals(ch2))
                    {
                        temp = 0;
                    }
                    else
                    {
                        temp = 1;
                    }
                    int current = LowerOfThree(GetMatrix(i - 1, j, stride) + 1,
                                               GetMatrix(i, j - 1, stride) + 1,
                                               GetMatrix(i - 1, j - 1, stride) + temp);
                    //if (current > early)
                    //{
                    //    return int.MaxValue;
                    //}
                    SetMatrix(i, j, stride, current);
                }
            }

            //for (i = 0; i <= n; i++)
            //{
            //    for (j = 0; j <= m; j++)
            //    {
            //        Console.Write(" {0} ", Matrix[i, j]);
            //    }
            //    Console.WriteLine("");
            //}

            return GetMatrix(n, m, stride);
        }

        public decimal CalcPercent(string str1, string str2, int val)
        {
            //int maxLenth = str1.Length > str2.Length ? str1.Length : str2.Length;
            return 1 - (decimal)val / Math.Max(str1.Length, str2.Length);
        }
    }

    class StringMatchResult
    {
        public int index;
        public string origin;
        public string target;
        public string pattern;
        public double flex;

        public StringMatchResult()
        {
            flex = -1.0;
            index = -1;
        }

        public class Camparer : IComparer<StringMatchResult>
        {
            int IComparer<StringMatchResult>.Compare(StringMatchResult x, StringMatchResult y)
            {
                return x.flex.CompareTo(y.flex);
            }
        }
    }

    class StringMatcher
    {
        static public Dictionary<int, string> LoadFullText(string filename)
        {
            return LoadFullText(filename, true);
        }

        static public Dictionary<int, string> LoadFullText(string filename, bool removeToken)
        {
            Regex entry = new Regex(@"^String #([0-9]+) is");

            Regex token = new Regex(@"\[[\w\s]+\]");

            char[] trims = { '~', ' ' };

            Dictionary<int, string> strings = new Dictionary<int, string>();
            int reading = -1;
            StringBuilder full = null;
            using (StreamReader sr = new StreamReader(filename))
            {
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();

                    Match m = entry.Match(line);
                    if (m.Success)
                    {
                        if (reading >= 0 && full != null)
                        {
                            string output = full.ToString();
                            if (removeToken)
                                output = token.Replace(full.ToString(), "");

                            strings.Add(reading, output.TrimEnd(trims));
                            full = null;
                        }
                        // next header
                        reading = int.Parse(m.Groups[1].ToString());
                    }

                    if (reading >= 0)
                    {
                        string content = line;
                        if (m.Success) // headline
                        {
                            full = new StringBuilder();
                            content = line.Substring(line.IndexOf('~') + 1);
                        }
                        else
                        {
                            // add the line break for previous line
                            full.AppendLine("");
                        }
                        full.Append(content);
                    }
                }
            }

            if (reading >= 0 && full != null)
            {
                strings.Add(reading, full.ToString());
                full = null;
            }

            return strings;
        }

        static public void SaveFullText(string filename, Dictionary<int, string> dic)
        {
            using (StreamWriter output = new StreamWriter(filename))
            {
                output.WriteLine(string.Format(@"// Total:{0}", dic.Count));
                foreach (var kv in dic)
                {
                    output.WriteLine(string.Format("String #{0} is ~{1}~", kv.Key, kv.Value));
                }
            }
        }

        Dictionary<int, string> _origin;
        Dictionary<int, string> _target;
        Dictionary<string, int> _escaped;
        Dictionary<string, int> _origin_rev;

        string[] _sorted;

        class StringLengthComparer : System.Collections.IComparer
        {
            public int Compare(object x, object y)
            {
                string lhs = x as string;
                string rhs = y as string;

                if (lhs.Length < rhs.Length)
                    return -1;
                else if (lhs.Length > rhs.Length)
                    return 1;

                return 0;
            }
        }

        public StringMatcher(string origin, string target)
        {
            _origin = LoadFullText(origin);
            _target = LoadFullText(target);

            _origin_rev = new Dictionary<string, int>();
            foreach (var kv in _origin)
            {
                if (!_origin_rev.ContainsKey(kv.Value))
                    _origin_rev.Add(kv.Value, kv.Key);
                //else
                //    Console.WriteLine(kv.Value);
            }

            _escaped = new Dictionary<string, int>();
            foreach (var kv in _origin_rev)
            {
                string ret = EscapeString(kv.Key);

                int duplicated = -1;
                if (_escaped.TryGetValue(ret, out duplicated))
                {
                    string beforeEscape = _origin[duplicated];
                    _escaped.Remove(ret);
                    _escaped.Add(beforeEscape, duplicated);
                    _escaped.Add(kv.Key, kv.Value);

                    //Console.WriteLine(string.Format("Conflict Previous: {0}, {1}", beforeEscape, duplicated));
                    //Console.WriteLine(string.Format("Conflict Current: {0}, {1}", kv.Key, kv.Value));
                }
                else
                {
                    _escaped.Add(ret, kv.Value);
                }
            }

            _sorted = new string[_escaped.Count];
            _escaped.Keys.CopyTo(_sorted, 0);

            Array.Sort(_sorted, new StringLengthComparer());
        }

        // English only
        static public string EscapeString(string input)
        {
            bool gotNonAlpha = false;
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < input.Length; i++)
            {
                char c = input[i];
                if ((c >= 'a' && c <= 'z') || c >= '0' && c <= '9')
                {
                    if (gotNonAlpha)
                    {
                        sb.Append(' ');
                        gotNonAlpha = false;
                    }
                    sb.Append(c);
                }
                else if (c >= 'A' && c <= 'Z')
                {
                    if (gotNonAlpha)
                    {
                        sb.Append(' ');
                        gotNonAlpha = false;
                    }
                    sb.Append((char)((c - 'A') + 'a'));
                    gotNonAlpha = false;
                }
                else
                {
                    gotNonAlpha = true;
                }
            }

            return sb.ToString();
        }

        private static int BinarySearch(string[] list, int value)
        {
            int lo = 0, hi = list.Length - 1;
            while (lo < hi)
            {
                int m = (hi + lo) / 2;  // this might overflow; be careful.
                if (list[m].Length < value) lo = m + 1;
                else hi = m - 1;
            }
            if (list[lo].Length < value) lo++;
            return lo;
        }

        public StringMatchResult Find(string pattern)
        {
            StringMatchResult r = new StringMatchResult();

            if (!SearchFromDict(_origin_rev, pattern, r))
            {
                string escaped = EscapeString(pattern);
                if (!SearchFromDict(_escaped, escaped, r))
                {
                    r = SearchFlexible(escaped);
                }
            }

            return r;
        }

        public void DoWork()
        {
            // Queue a task.
            System.Threading.ThreadPool.QueueUserWorkItem(
                new System.Threading.WaitCallback(SomeLongTask));
            // Queue another task.
            System.Threading.ThreadPool.QueueUserWorkItem(
                new System.Threading.WaitCallback(AnotherLongTask));
        }

        private void SomeLongTask(Object state)
        {
            // Insert code to perform a long task.
        }

        private void AnotherLongTask(Object state)
        {
            // Insert code to perform a long task.
        }

        private StringMatchResult SearchFlexible(string escaped)
        {
            // flexible matching
            int delta = Math.Min((int)(escaped.Length * 0.5), 100); // no more than 20 char
            int distanceBegin = Math.Max(1, escaped.Length - delta);
            int distanceEnd = escaped.Length + delta;

            int indexBegin = BinarySearch(_sorted, distanceBegin);
            int indexEnd = BinarySearch(_sorted, distanceEnd);
            if (indexEnd >= _sorted.Length) indexEnd = _sorted.Length - 1;

            // use 8 threads
            int tasks = Math.Max(1, ((indexEnd - indexBegin) + 1) / 8);
            List<StringMatchResult> results = new List<StringMatchResult>();
            List<ManualResetEvent> doneEvents = new List<ManualResetEvent>();


            while (indexBegin <= indexEnd)
            {
                int currentBegin = indexBegin;
                int currentEnd = currentBegin + tasks - 1;
                if (currentEnd > indexEnd)
                    currentEnd = indexEnd;

                StringMatchResult currentResult = new StringMatchResult();
                results.Add(currentResult);

                ManualResetEvent doneEvent = new ManualResetEvent(false);
                doneEvents.Add(doneEvent);

                // Queue another task.
                ThreadPool.QueueUserWorkItem(new WaitCallback(delegate(Object state)
                {
                    SearchFlexibleWorker(escaped, currentResult, distanceEnd, currentBegin, currentEnd);
                    doneEvent.Set();
                }));

                indexBegin = currentEnd + 1;
            }

            // failed directly
            if (doneEvents.Count == 0)
            {
                return new StringMatchResult();
            }

            foreach (ManualResetEvent doneEvent in doneEvents)
            {
                doneEvent.WaitOne();
            }
            //WaitHandle.WaitAll(doneEvents.ToArray());

            results.Sort(new StringMatchResult.Camparer());

            return results[results.Count - 1];
        }

        // Reuse the calculator to avoid out-of-memory
        static Stack<LevenshteinDistance> calculators = new Stack<LevenshteinDistance>();
        static private LevenshteinDistance PopCalculator()
        {
            lock (calculators)
            {
                if (calculators.Count == 0)
                {
                    return new LevenshteinDistance();
                }
                return calculators.Pop();
            }
        }
        static private void PushCalculator(LevenshteinDistance used)
        {
            lock (calculators)
            {
                calculators.Push(used);
            }
        }

        private void SearchFlexibleWorker(string escaped, StringMatchResult r, int distanceEnd, int indexBegin, int indexEnd)
        {
            LevenshteinDistance distance = PopCalculator();

            string found = null;
            int minDistance = distanceEnd;
            for (int i = indexEnd; i >= indexBegin; i--)
            {
                string current = _sorted[i];
                int dis = distance.Calc(escaped, current, minDistance);
                if (dis < minDistance)
                {
                    found = current;
                    minDistance = dis;
                }
            }

            if (found != null)
            {
                int index = _escaped[found];
                if (_target.ContainsKey(index))
                {
                    r.flex = (double)distance.CalcPercent(found, escaped, minDistance);
                    r.index = index;
                    r.target = _target[index];
                    r.origin = _origin[index];
                    r.pattern = escaped;
                }
            }

            PushCalculator(distance);
        }

        private bool SearchFromDict(Dictionary<string, int> dic, string pattern, StringMatchResult r)
        {
            if (dic.TryGetValue(pattern, out r.index))
            {
                if (_target.TryGetValue(r.index, out r.target))
                {
                    r.origin = _origin[r.index];
                    r.pattern = pattern;
                    return true;
                }
            }

            r.index = -1;
            return false;
        }
    }
    #endregion

    interface ILogger
    {
        void AppendLog(string content);
    }

    class Translator
    {
        StringMatcher _matcher;
        ILogger _external_logger;

        // to export the final result
        Dictionary<string, string> _misMatched = new Dictionary<string, string>();
        List<string> _misMatchedSorted = new List<string>();

        // clear every module
        int _indexCounter = 0;
        int _refCounter = 0;
        int _missCounter = 0;
        int _flexibleCounter = 0;

        bool _isReadonly = false;
        bool _skipTranslation = false;
        bool _skipCampaign = false;

        bool _includeJournal = false;
        bool _includeBlueprint = false;
        bool _includeConversation = false;

        public Translator(string origin, string target, ILogger logger, bool isReadonly,
                          bool skipTranslation, bool skipCampaign, 
                          bool includeJournal, bool includeBlueprint, bool includeConversation)
        {
            _external_logger = logger;
            _matcher = new StringMatcher(origin, target);
            _isReadonly = isReadonly;
            _skipTranslation = skipTranslation;
            _skipCampaign = skipCampaign;

            _includeJournal = includeJournal;
            _includeBlueprint = includeBlueprint;
            _includeConversation = includeConversation;
        }

        public void ConvertConversation(string root, string[] modules, string missLog, string outputLog)
        {
            bool didCampaign = _skipCampaign; // true to skip campaign

            _external_logger.AppendLog("================================");

            // store the previous miss log
            if (File.Exists(missLog))
            {
                var previous = StringMatcher.LoadFullText(missLog, false); // keep token such as [TOKEN]
                _external_logger.AppendLog(string.Format("Loading previous {0}...", previous.Count));
                foreach (var kv in previous) // 
                {
                    StoreMismatch(kv.Value, "previous");
                }
            }

            using (StreamWriter logger = new StreamWriter(outputLog))
            {
                foreach (string moduleName in modules)
                {
                    _external_logger.AppendLog(string.Format("Loading module {0}...", moduleName));

                    if (moduleName.Contains(".mod"))
                    {
                        NWN2Toolset.NWN2ToolsetMainForm.App.Module.OpenModuleFile(root + moduleName);
                    }
                    else
                    {
                        NWN2Toolset.NWN2ToolsetMainForm.App.Module.OpenModuleDirectory(root + moduleName);
                    }

                    if (!didCampaign)
                    {
                        _external_logger.AppendLog(string.Format("Loading campaign ..."));

                        didCampaign = true;

                        if (_includeJournal)
                            ExportJournal(logger, "Journal-Campaign", NWN2Toolset.NWN2.Data.Campaign.NWN2CampaignManager.Instance.ActiveCampaign.Journal);

                        if(_includeBlueprint)
                            ExportBlueprintSet(logger, "Blueprint-Campaign", NWN2Toolset.NWN2.Data.Campaign.NWN2CampaignManager.Instance.ActiveCampaign);

                        if (_includeConversation)
                        {
                            var campaign_convs = NWN2Toolset.NWN2.Data.Campaign.NWN2CampaignManager.Instance.ActiveCampaign.Conversations;
                            foreach (string key in campaign_convs.Keys)
                            {
                                NWN2GameConversation conv = campaign_convs[key];
                                ExportConv(logger, key, conv);
                            }
                        }

                        _external_logger.AppendLog(string.Format("Campaign done..."));
                    }

                    if (_includeJournal)
                        ExportJournal(logger, "Journal-" + moduleName, NWN2Toolset.NWN2ToolsetMainForm.App.Module.Journal);

                    if (_includeBlueprint)
                        ExportBlueprintSet(logger, "Blueprint-" + moduleName, NWN2Toolset.NWN2ToolsetMainForm.App.Module);

                    if (_includeConversation)
                    {
                        var convs = NWN2Toolset.NWN2ToolsetMainForm.App.Module.Conversations;
                        foreach (string key in convs.Keys)
                        {
                            NWN2GameConversation conv = convs[key];
                            ExportConv(logger, key, conv);
                        }
                    }

                    _external_logger.AppendLog(string.Format("Module {0} done...", moduleName));

                    NWN2Toolset.NWN2ToolsetMainForm.App.Module.CloseModule();
                    logger.Flush();

                    // update this after every module in case we crash... :(
                    using (StreamWriter miss = new StreamWriter(missLog))
                    {
                        miss.WriteLine(string.Format(@"// Total:{0}", _misMatched.Count));
                        int i = 0x2000000;
                        foreach (var dialog in _misMatchedSorted)
                        {
                            miss.WriteLine(string.Format("String #{0} is ~{1}~", i++, dialog));
                        }
                    }
                }
            }
        }

        private void ExportBlueprintSet(StreamWriter logger, string name, INWN2BlueprintSet set)
        {
            ExportBlueprints(logger, "Creatures-" + name, set.Creatures);
            ExportBlueprints(logger, "Doors-" + name, set.Doors);
            ExportBlueprints(logger, "Encounters-" + name, set.Encounters);
            ExportBlueprints(logger, "EnvironmentObjects-" + name, set.EnvironmentObjects);
            ExportBlueprints(logger, "Items-" + name, set.Items);
            ExportBlueprints(logger, "Placeables-" + name, set.Placeables);
            ExportBlueprints(logger, "PlacedEffects-" + name, set.PlacedEffects);
            ExportBlueprints(logger, "Sounds-" + name, set.Sounds);
            ExportBlueprints(logger, "StaticCameras-" + name, set.StaticCameras);
            ExportBlueprints(logger, "Stores-" + name, set.Stores);
            ExportBlueprints(logger, "Trees-" + name, set.Trees);
            ExportBlueprints(logger, "Triggers-" + name, set.Triggers);
            ExportBlueprints(logger, "Waypoints-" + name, set.Waypoints);
        }

        private void ExportBlueprints(StreamWriter logger, string name, NWN2BlueprintCollection blueprint)
        {
            _external_logger.AppendLog(string.Format("Exporting Blueprint {0}...", name));

            StringBuilder sb = new StringBuilder();

            // clear every module's blueprint
            _indexCounter = 0;
            _refCounter = 0;
            _missCounter = 0;
            _flexibleCounter = 0;

            int capacity = 0;
            foreach (INWN2Object cat in blueprint)
            {
                StoreConv(sb, cat.LocalizedName, name);
                capacity++;
                StoreConv(sb, cat.LocalizedDescription, name);
                capacity++;

                if (!_isReadonly)
                {
                    IOEISerializable s = cat as IOEISerializable;
                    INWN2Blueprint b = cat as INWN2Blueprint;
                    if (s != null && b != null)
                    {
                        s.OEISerialize(b.Resource.GetStream(true));
                    }
                }
            }

            logger.WriteLine(string.Format(@"// Blueprint:{0}", name));
            string summary = (string.Format("// Ref:{0},Entries:{1},Mismatch:{2},Flex:{3}",
                                            _refCounter, capacity, _missCounter, _flexibleCounter));
            logger.WriteLine(summary);
            logger.Write(sb.ToString());

            _external_logger.AppendLog(summary);
            _external_logger.AppendLog(string.Format("Journal {0} done.", name));
        }

        private void ExportJournal(StreamWriter logger, string name, NWN2Journal journal)
        {
            _external_logger.AppendLog(string.Format("Exporting journal {0}...", name));

            StringBuilder sb = new StringBuilder();

            // clear every module's journal
            _indexCounter = 0;
            _refCounter = 0;
            _missCounter = 0;
            _flexibleCounter = 0;

            int capacity = 0;
            foreach (NWN2JournalCategory cat in journal.Categories)
            {
                StoreConv(sb, cat.Name, name);
                capacity++;
                foreach (NWN2JournalEntry entry in cat.Entries)
                {
                    StoreConv(sb, entry.Text, name);
                    capacity++;
                }
            }

            logger.WriteLine(string.Format(@"// Journal:{0}", name));
            string summary = (string.Format("// Ref:{0},Entries:{1},Mismatch:{2},Flex:{3}",
                                            _refCounter, capacity, _missCounter, _flexibleCounter));
            logger.WriteLine(summary);
            logger.Write(sb.ToString());

            _external_logger.AppendLog(summary);
            _external_logger.AppendLog(string.Format("Journal {0} done.", name));

            if (!_isReadonly)
            {
                journal.OEISerialize(journal.Resource.GetStream(true));
            }
        }

        private void ExportConv(StreamWriter logger, string name, NWN2GameConversation conv)
        {
            _external_logger.AppendLog(string.Format("Exporting conv {0}...", name));

            _indexCounter = 0;
            _refCounter = 0;
            _missCounter = 0;
            _flexibleCounter = 0;

            StringBuilder sb = new StringBuilder();

            if (!conv.Loaded)
            {
                conv.OEIUnserialize(conv.Resource.GetStream(false));
            }

            foreach (NWN2ConversationLine line in conv.Entries)
            {
                StoreConv(sb, line.Text, name);
            }

            foreach (NWN2ConversationLine line in conv.Replies)
            {
                StoreConv(sb, line.Text, name);
            }

            logger.WriteLine(string.Format(@"// Conversion:{0}", name));
            string summary = (string.Format("// Ref:{0},Loaded:{1},Entries:{2},Replies:{3},Mismatch:{4},Flex:{5}",
                                            _refCounter, conv.Loaded, conv.Entries.Capacity, conv.Replies.Capacity, _missCounter, _flexibleCounter));
            logger.WriteLine(summary);
            logger.Write(sb.ToString());

            _external_logger.AppendLog(summary);
            _external_logger.AppendLog(string.Format("Conv {0} done.", name));

            if (!_isReadonly)
            {
                conv.HasChanged = true;
                conv.OEISerialize(conv.Resource.GetStream(true));
                conv.Release();
            }
        }

        void StoreMismatch(string dialog, string name)
        {
            if(!_misMatched.ContainsKey(dialog))
            {
                _misMatched.Add(dialog, name);
                _misMatchedSorted.Add(dialog);
            }

            _missCounter++;
        }

        private void StoreConv(StringBuilder sb, OEIExoLocString Text, string name)
        {
            _refCounter += Text.StringRefValid ? 1 : 0;

            string dialog = Text[OEIShared.Utils.BWLanguages.BWLanguage.English];
            if (dialog.Length == 0 || dialog.Trim().Length == 0)
            {
                // skip empty quote
                return;
            }

            if (_indexCounter % 100 == 0)
                _external_logger.AppendLog(string.Format("Trans line {0}...", _indexCounter));


            if (_skipTranslation)
            {
                StoreMismatch(dialog, name);
            }
            else
            {
                var result = _matcher.Find(dialog);
                if (result.index >= 0)
                {
                    if (result.flex > 0)
                    {
                        if (result.flex > 0.85)
                        {
                            dialog = result.target;
                            _flexibleCounter++;
                            if (!_isReadonly)
                                Text[OEIShared.Utils.BWLanguages.BWLanguage.English] = dialog;
                        }
                        else
                        {
                            StoreMismatch(dialog, name);
                        }
                    }
                    else
                    {
                        // normal matching
                        dialog = result.target;
                        if (!_isReadonly)
                            Text[OEIShared.Utils.BWLanguages.BWLanguage.English] = dialog;
                    }
                }
                else
                {
                    StoreMismatch(dialog, name);
                }
            }

            // do not record string ref is there is nothing
            string refIndex = Text.StringRef != 0xFFFFFFFF ?
                string.Format(",0x{0:X}", Text.StringRef) : "";

            sb.AppendLine(string.Format("String #{0}{1} is ~{2}~",
                          _indexCounter,
                          refIndex,
                          dialog));

            _indexCounter++;
        }
    }
}
