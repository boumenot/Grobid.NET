using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

using Xunit;

using Grobid.NET;

namespace Grobid.Test
{
    public class TeiFeatureMergeBatchTest
    {
        //[Fact]
        [Fact(Skip = "works on my machine only")]
        [Trait("Category", "EndToEnd")]
        public void Test()
        {
            var teiFeatureMerge = new TeiFeatureMerge();
            var teiFeatureFactory = new TeiFeatureFactory();

            var mergedCorpus = Directory.EnumerateFiles(@"c:\temp\corpus\header\label", "*.header")
                .Select(x => new {
                    HeaderFullName = x,
                    TeiFullName = Path.Combine(@"c:\temp\corpus\header\tei", $"{Path.GetFileNameWithoutExtension(x)}.tei"),
                })
                .Select(x => new
                {
                    TeiFeatures = teiFeatureFactory.Create(XDocument.Load(File.OpenRead(x.TeiFullName))),
                    FeatureRows = GetFeatureRows(x.HeaderFullName),
                })
                .SelectMany(x => teiFeatureMerge.Merge(x.TeiFeatures, x.FeatureRows))
                .ToArray();

            var sb = new StringBuilder();
            var scorer = new ModelScorer();
            foreach (var x in mergedCorpus)
            {
                scorer.Eval(x.Expected, x.Actual);
            }

            sb.AppendFormat("{0,16} {1,10} {2,10} {3,10} {4,10}\n",
                "Label",
                "Accuracy",
                "Precision",
                "Recall",
                "F0");

            foreach (var label in scorer.Scores().OrderBy(x => x.Label))
            {
                sb.AppendFormat("{0,16} {1,10:N3} {2,10:N3} {3,10:N3} {4,10:N3}\n",
                    label.Label,
                    label.Accuracy,
                    label.Precision,
                    label.Recall,
                    label.F0);
            }

            var cumulativeScorer = CumulativeScore.Create(scorer.Scores().ToArray());
            sb.AppendLine();
            sb.AppendFormat("{0,16} {1,10:N3} {2,10:N3} {3,10:N3} {4,10:N3}\n",
                "MICRO AVG.",
                cumulativeScorer.MicroLabelScore.Accuracy,
                cumulativeScorer.MicroLabelScore.Precision,
                cumulativeScorer.MicroLabelScore.Recall,
                cumulativeScorer.MicroLabelScore.F0);

            sb.AppendFormat("{0,16} {1,10:N3} {2,10:N3} {3,10:N3} {4,10:N3}\n",
                "MACRO AVG.",
                cumulativeScorer.MacroLabelScore.Accuracy,
                cumulativeScorer.MacroLabelScore.Precision,
                cumulativeScorer.MacroLabelScore.Recall,
                cumulativeScorer.MacroLabelScore.F0);

            var s = sb.ToString();
        }

        private static FeatureRow[] GetFeatureRows(string fileName)
        {
            var featureRows = File.ReadAllLines(fileName)
              .Select(x => x.Trim())
              .Where(x => !String.IsNullOrWhiteSpace(x))
              .Select(FeatureRow.Parse)
              .ToArray();

            return featureRows;
        }
    }
}
