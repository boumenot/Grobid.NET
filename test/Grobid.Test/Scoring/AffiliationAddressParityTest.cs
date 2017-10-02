using System.Linq;
using System.Reflection;
using System.Text;
using ApprovalTests;
using ApprovalTests.Reporters;
using Grobid.NET.Scoring;
using Xunit;

namespace Grobid.Test.Scoring
{
    // Evaluate the parity between Grobid.NET and Grobid for the Affiliation-Address model.
    [UseReporter(typeof(BeyondCompare4Reporter))]
    public class AffiliationAddressParityTest 
    {
        [Fact]
        public void AffiliationAddressModelComparisonAgainstGrobid()
        {
            var features = Assembly.GetExecutingAssembly().GetManifestResourceStream("Grobid.Test.content.official.affiliation-address.labeled.txt")
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

    }
}
