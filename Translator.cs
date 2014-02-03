using NWN2Toolset.NWN2.Data;
using NWN2Toolset.NWN2.Data.ConversationData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace FDRConversationTranslator
{
    #region String Matching
    public class LevenshteinDistance
    {
        static private int LowerOfThree(int first, int second, int third)
        {
            int min = Math.Min(first, second);
            return Math.Min(min, third);
        }

        static public int step = 0;
        static public int Calc(string str1, string str2, int early)
        {
            step = 0;

            int[,] Matrix;
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
            Matrix = new int[n + 1, m + 1];

            for (i = 0; i <= n; i++)
            {
                //初始化第一列
                Matrix[i, 0] = i;
            }

            for (j = 0; j <= m; j++)
            {
                //初始化第一行
                Matrix[0, j] = j;
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
                    int current = LowerOfThree(Matrix[i - 1, j] + 1, Matrix[i, j - 1] + 1, Matrix[i - 1, j - 1] + temp);
                    //if (current > early)
                    //{
                    //    return int.MaxValue;
                    //}
                    Matrix[i, j] = current;
                    step++;
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

            return Matrix[n, m];
        }

        static public decimal CalcPercent(string str1, string str2, int val)
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

                    Console.WriteLine(string.Format("Conflict Previous: {0}, {1}", beforeEscape, duplicated));
                    Console.WriteLine(string.Format("Conflict Current: {0}, {1}", kv.Key, kv.Value));
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
            r.index = -1;

            if (!SearchFromDict(_origin_rev, pattern, r))
            {
                string escaped = EscapeString(pattern);
                if (!SearchFromDict(_escaped, escaped, r))
                {
                    SearchFlexible(escaped, r);
                }
            }

            return r;
        }

        private void SearchFlexible(string escaped, StringMatchResult r)
        {
            // flexible matching
            int delta = Math.Min((int)(escaped.Length * 0.5), 20); // no more than 20 char
            int distanceBegin = Math.Max(1, escaped.Length - delta);
            int distanceEnd = escaped.Length + delta;

            int indexBegin = BinarySearch(_sorted, distanceBegin);
            int indexEnd = BinarySearch(_sorted, distanceEnd);

            string found = null;
            int minDistance = distanceEnd;
            for (int i = indexBegin; i <= indexEnd; i++)
            {
                string current = _sorted[i];
                int dis = LevenshteinDistance.Calc(escaped, current, minDistance);
                if (dis < minDistance)
                {
                    found = current;
                    minDistance = dis;
                }
            }

            if (found != null)
            {
                r.flex = (double)LevenshteinDistance.CalcPercent(found, escaped, minDistance);
                int index = _escaped[found];
                r.index = index;
                r.target = _target[index];
                r.origin = _origin[index];
                r.pattern = escaped;
            }
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

        public Translator(string origin, string target, ILogger logger, bool isReadonly)
        {
            _external_logger = logger;
            _matcher = new StringMatcher(origin, target);
            _isReadonly = isReadonly;
        }

        public void ConvertConversation(string root, string[] modules, bool skipCampaign, string missLog, string outputLog)
        {
            bool didCampaign = skipCampaign; // true to skip campaign

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

                        var campaign_convs = NWN2Toolset.NWN2.Data.Campaign.NWN2CampaignManager.Instance.ActiveCampaign.Conversations;
                        foreach (string key in campaign_convs.Keys)
                        {
                            NWN2GameConversation conv = campaign_convs[key];
                            ExportConv(logger, key, conv);
                        }

                        _external_logger.AppendLog(string.Format("Campaign done..."));
                    }

                    var convs = NWN2Toolset.NWN2ToolsetMainForm.App.Module.Conversations;
                    foreach (string key in convs.Keys)
                    {
                        NWN2GameConversation conv = convs[key];
                        ExportConv(logger, key, conv);
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
                StoreConv(sb, line, name);
            }

            foreach (NWN2ConversationLine line in conv.Replies)
            {
                StoreConv(sb, line, name);
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

        private void StoreConv(StringBuilder sb, NWN2ConversationLine line, string name)
        {
            _refCounter += line.Text.StringRefValid ? 1 : 0;

            string dialog = line.Text[OEIShared.Utils.BWLanguages.BWLanguage.English];
            if (dialog.Length == 0 || dialog.Trim().Length == 0)
            {
                // skip empty quote
                return;
            }

            if (_indexCounter % 100 == 0)
                _external_logger.AppendLog(string.Format("Trans line {0}...", _indexCounter));

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
                            line.Text[OEIShared.Utils.BWLanguages.BWLanguage.English] = dialog;
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
                        line.Text[OEIShared.Utils.BWLanguages.BWLanguage.English] = dialog;
                }
            }
            else
            {
                StoreMismatch(dialog, name);
            }

            sb.AppendLine(string.Format("String #{0},{1}, is ~{2}~",
                          _indexCounter,
                          line.Text.StringRef,
                          dialog));

            _indexCounter++;
        }
    }
}
