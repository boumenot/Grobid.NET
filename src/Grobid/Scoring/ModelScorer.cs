using System.Collections.Generic;
using System.Linq;

namespace Grobid.NET.Scoring
{
    public sealed class ModelScorer
    {
        public ModelScorer()
        {
            this.Stat = new ModelStat();
        }

        public ModelStat Stat { get; }

        public void Eval(string expected, string obtained)
        {
            bool isNewLabel = this.IsNewLabel(expected);

            if (expected == obtained)
            {
                this.Stat.IncrExpected(expected);
                this.Stat.IncrObserved(expected);
            }
            else
            {
                this.Stat.IncrFalseNegative(expected);
                this.Stat.IncrFalsePositive(obtained);

                var l = isNewLabel ? obtained : expected;
                this.Stat.IncrExpected(l);
            }
        }

        private bool IsNewLabel(string expected)
        {
            return !this.Stat.Labels.Contains(expected);
        }

        public IEnumerable<LabelScore> Scores()
        {
            var total = this.Stat.LabelStats.Values.Sum(x => x.Expected + x.FalsePositive);
            foreach (var label in this.Stat.Labels)
            {
                var labelStat = this.Stat.LabelStats[label];
                yield return new LabelScore(label, total, labelStat);
            }
        }
    }
}