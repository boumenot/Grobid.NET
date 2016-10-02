using System;

namespace Grobid.NET
{
    public class FeatureRow {
        public string Value { get; set; }
        public string Classification { get; set; }
        public bool IsStart { get; set; }

        public static FeatureRow Parse(string s)
        {
            var classification = FeatureRow.ExtractClassification(s);

            var featureRow = new FeatureRow
            {
                Classification = classification.Item1,
                IsStart = classification.Item2,
                Value = FeatureRow.ExtractValue(s),
            };

            return featureRow;
        }

        private static Tuple<string, bool> ExtractClassification(string s)
        {
            int begin = s.LastIndexOf("<");
            if (begin == -1)
            {
                return Tuple.Create(string.Empty, false);
            }

            int end = s.LastIndexOf(">");
            if (end == -1)
            {
                throw new ArgumentException($"Cannot locate end of classification token in '{s}'");
            }

            if (begin > end)
            {
                throw new ArgumentException($"Cannot classification token in '{s}'");
            }

            bool isStart = begin >= 2 && s[begin - 2] == 'I' && s[begin - 1] == '-';
            // +1 == eat extraneous '<'
            // -1 == eat extraneous '>'
            var classification = s.Substring(begin + 1, end - begin - 1);

            return Tuple.Create(classification, isStart);
        }

        private static string ExtractValue(string s)
        {
            int end = s.IndexOf(" ");
            if (end == -1)
            {
                throw new ArgumentException($"Cannot locate value in'{s}'");
            }

            return s.Substring(0, end);
        }
    }
}