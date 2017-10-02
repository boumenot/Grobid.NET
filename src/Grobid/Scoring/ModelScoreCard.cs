using System.Linq;
using System.Text;

namespace Grobid.NET.Scoring
{
    public class ModelScoreCard
    {
        public string Render(ModelStat modelStat)
        {
            var sb = new StringBuilder();
            this.Render(modelStat, sb);

            return sb.ToString();
        }

        public void Render(ModelStat modelStat, StringBuilder sb)
        {
            //var total = 0;
            //foreach (var kvp in modelStat.LabelStats)
            //{
            //    total += kvp.Value.Expected;
            //    total += kvp.Value.FalsePositive;

            //    //sb.AppendLine($"{kvp.Key}=expected={kvp.Value.Expected}");
            //    //sb.AppendLine($"{kvp.Key}=falsePositive={kvp.Value.FalsePositive}");
            //}

            //sb.AppendLine($"total={total}");
            //foreach (var kvp in scorer.Stat.LabelStats)
            //{
            //    sb.AppendLine($"{kvp.Key} tp={kvp.Value.Observed}");
            //    sb.AppendLine($"{kvp.Key} fp={kvp.Value.FalsePositive}");
            //    sb.AppendLine($"{kvp.Key} fn={kvp.Value.FalseNegative}");
            //    var tn = total - kvp.Value.Observed - (kvp.Value.FalsePositive + kvp.Value.FalseNegative);
            //    sb.AppendLine($"{kvp.Key} tn={tn}");
            //    sb.AppendLine($"{kvp.Key} all={kvp.Value.Expected}");
            //}

            sb.AppendFormat("{0,16} {1,10} {2,10} {3,10} {4,10}\n",
                "Label",
                "Accuracy",
                "Precision",
                "Recall",
                "F0");

            foreach (var label in modelStat.Scores().OrderBy(x => x.Label))
            {
                sb.AppendFormat("{0,16} {1,10:N4} {2,10:N4} {3,10:N4} {4,10:N4}\n",
                    label.Label,
                    label.Accuracy,
                    label.Precision,
                    label.Recall,
                    label.F0);
            }

            var cumulativeScorer = CumulativeScore.Create(modelStat.Scores().ToArray());
            sb.AppendLine();
            sb.AppendFormat("{0,16} {1,10:N4} {2,10:N4} {3,10:N4} {4,10:N4}\n",
                "MICRO AVG.",
                cumulativeScorer.MicroLabelScore.Accuracy,
                cumulativeScorer.MicroLabelScore.Precision,
                cumulativeScorer.MicroLabelScore.Recall,
                cumulativeScorer.MicroLabelScore.F0);

            sb.AppendFormat("{0,16} {1,10:N4} {2,10:N4} {3,10:N4} {4,10:N4}\n",
                "MACRO AVG.",
                cumulativeScorer.MacroLabelScore.Accuracy,
                cumulativeScorer.MacroLabelScore.Precision,
                cumulativeScorer.MacroLabelScore.Recall,
                cumulativeScorer.MacroLabelScore.F0);
        }
    }
}
