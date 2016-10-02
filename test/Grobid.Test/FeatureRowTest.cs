using System;

using FluentAssertions;
using Xunit;

namespace Grobid.Test
{
    public class FeatureRowTest
    {
        [Fact]
        public void FeatureRow00()
        {
            var testSubject = FeatureRow.Parse("The the 1 1 0 1 0 LINESTART 0 1 I-<title>");

            testSubject.Classification.Should().Be("title");
            testSubject.Value.Should().Be("The");
            testSubject.IsStart.Should().BeTrue();
        }

        [Fact]
        public void FeatureRow01()
        {
            var testSubject = FeatureRow.Parse("The the 1 1 0 1 0 LINESTART 0 1 <title>");

            testSubject.Classification.Should().Be("title");
            testSubject.Value.Should().Be("The");
            testSubject.IsStart.Should().BeFalse();
        }

        [Fact]
        public void FeatureRow02()
        {
            var testSubject = FeatureRow.Parse("The the 1 1 0 1 0 LINESTART 0 1 <do-not-select> <title>");

            testSubject.Classification.Should().Be("title");
            testSubject.Value.Should().Be("The");
            testSubject.IsStart.Should().BeFalse();
        }

        [Fact]
        public void FeatureRow03()
        {
            var malformedFeatureRows = new[]
            {
                "The the 1 1 0 1 0 LINESTART 0 1",
                "The the 1 1 0 1 0 LINESTART 0 1 <title",
                "The the 1 1 0 1 0 LINESTART 0 1 title>",
                "The the 1 1 0 1 0 LINESTART 0 1 >title<",
                "The\tthe\t1\t1\t0\t1\t0\tLINESTART\t0\t1\t<title>",
                "<title>",
            };

            foreach (var malformedFeatureRow in malformedFeatureRows)
            {
                Action action = () => FeatureRow.Parse(malformedFeatureRow);
                action.ShouldThrow<ArgumentException>();
            }
        }
    }

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
                throw new ArgumentException($"Cannot locate start of classification token in '{s}'");
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
