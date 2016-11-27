using System;

namespace Grobid.NET.Wapiti
{
    public sealed class TrainerRow {
        public string Expected { get; set; }
        public string Obtained { get; set; }
        public string Value { get; set; }

        public static TrainerRow Parse(string s)
        {
            var expected = TrainerRow.ExtractClassification(s, 0);
            var obtained = TrainerRow.ExtractClassification(s, expected.Item2 + 1);

            var row = new TrainerRow
            {
                Expected = expected.Item1,
                Obtained = obtained.Item1,
                Value = TrainerRow.ExtractValue(s),
            };

            return row;
        }

        private static Tuple<string, int> ExtractClassification(string s, int startingIndex)
        {
            int begin = s.IndexOf("<", startingIndex);
            if (begin == -1)
            {
                throw new ArgumentException($"Cannot locate end of classification token in '{s}'");
            }

            int end = s.IndexOf(">", begin);
            if (end == -1)
            {
                throw new ArgumentException($"Cannot locate end of classification token in '{s}'");
            }

            // +1 == eat extraneous '<'
            // -1 == eat extraneous '>'
            var classification = s.Substring(begin + 1, end - begin - 1);
            return Tuple.Create(classification, begin);
        }

        private static string ExtractValue(string s)
        {
            int end = s.IndexOf(" ");
            if (end == -1 || string.IsNullOrWhiteSpace(s) || s[0] == '<')
            {
                throw new ArgumentException($"Cannot locate value in'{s}'");
            }

            return s.Substring(0, end);
        }
    }
}