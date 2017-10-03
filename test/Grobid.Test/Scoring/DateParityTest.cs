using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

using ApprovalTests;
using ApprovalTests.Reporters;
using Xunit;

using Grobid.NET.Scoring;

namespace Grobid.Test.Scoring
{
    // Evaluate the parity between Grobid.NET and Grobid for the Date model.
    [UseReporter(typeof(BeyondCompare4Reporter))]
    public class DateParityTest
    {
        public static Regex LabeledRx = new Regex(@"(?:I-)?<(\S+)>\t(?:I-)?<(\S+)>", RegexOptions.Compiled);

        // Compare the accuracy of our Grobid.NET's scoring vs. Grobid's scoring.
        [Fact]
        public void DateModelComparisonAgainstGrobid()
        {
            var features = Assembly.GetExecutingAssembly().GetManifestResourceStream("Grobid.Test.content.official.date.labeled.txt")
                .ToLines()
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => new
                {
                    Line = x,
                    Match = DateParityTest.LabeledRx.Match(x)
                })
                .Select(x => new
                {
                    Expected = x.Match.Groups[1].Value,
                    Actual = x.Match.Groups[2].Value,
                    x.Line,
                })
                .ToArray();

            var sb = new StringBuilder();
            var scorer = new ModelScorer();
            foreach (var x in features)
            {
                scorer.Eval(x.Expected, x.Actual);
            }

            sb.AppendLine($"==== Token Level Result ====\n");
            var modelScoreCard = new ModelScoreCard();
            modelScoreCard.Render(scorer.Stat, sb);

            var s = sb.ToString();
            Approvals.Verify(sb);
        }

        // This test should the *same* result as the test above (DateModelComparisonAgainstGrobid).
        // The point is to validate that our model when applied to the same corpus produces the 
        // *same* result.  It validates that the C# bindings for Wapiti are functional.
        [Fact]
        public void DateModelComparisonAgainstGrobidWithLabeling()
        {
            var model = global::Wapiti.Wapiti.Load(@"content\models\date\model.wapiti");
            var labeled = model.Label(Assembly.GetExecutingAssembly().GetManifestResourceStream("Grobid.Test.content.official.date.txt"));

            var features = labeled.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => new
                {
                    Match = DateParityTest.LabeledRx.Match(x)
                })
                .Select(x => new
                {
                    Expected = x.Match.Groups[1].Value,
                    Actual = x.Match.Groups[2].Value,
                })
                .ToArray();

            var sb = new StringBuilder();
            var scorer = new ModelScorer();
            foreach (var x in features)
            {
                scorer.Eval(x.Expected, x.Actual);
            }

            sb.AppendLine($"==== Token Level Result ====\n");
            var modelScoreCard = new ModelScoreCard();
            modelScoreCard.Render(scorer.Stat, sb);

            var s = sb.ToString();
            Approvals.Verify(sb);
        }

        private static ModelStat ScoreFieldStats(IEnumerable<Tuple<string, string>> featuresX, StringBuilder sb)
        {
            // XXX(chrboum) - actually this is much more complicated than it looks.  I will leave
            // this style of scoring for another day... (I am not sure how important it even is).
            var features = featuresX
                .Select(x => new
                {
                    Expected = x.Item1,
                    Actual = x.Item2,
                })
                .ToArray();

            var fieldStat = new ModelStat();
            bool allGood = true;
            foreach (var feature in features.Zip(features.Skip(1), (x, y) => new { Prev = x, Current = y }))
            {
                if (feature.Prev.Expected != feature.Current.Expected)
                {
                    sb.AppendFormat("[1]: lastPreviousToken={0}, previousToken={1}, allGood={2}\n", feature.Prev.Expected, feature.Current.Expected, allGood);

                    if (allGood)
                    {
                        fieldStat.IncrObserved(feature.Current.Expected);
                    }
                    else
                    {
                        fieldStat.IncrFalseNegative(feature.Current.Expected);
                    }
                    fieldStat.IncrExpected(feature.Current.Expected);
                }

                if (feature.Prev.Actual != feature.Current.Actual)
                {
                    sb.AppendFormat("[2]: lastCurrentToken={0}, currentToken={1}, allGood={2}\n", feature.Prev.Actual, feature.Current.Actual, allGood);
                    if (!allGood)
                    {
                        fieldStat.IncrFalsePositive(feature.Current.Actual);
                    }
                }

                if (feature.Prev.Actual   != feature.Current.Actual ||
                    feature.Prev.Expected != feature.Current.Expected)
                {
                    sb.AppendFormat("[3]: allGood {0} -> true\n", allGood);
                    allGood = true;
                }

                if (feature.Current.Expected != feature.Current.Actual)
                {
                    sb.AppendFormat("[3]: allGood {0} -> false\n", allGood);
                    allGood = false;
                }
            }

            return fieldStat;
        }
    }
}
