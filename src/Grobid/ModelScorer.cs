using System.Linq;

namespace Grobid.NET
{
    public class ModelScorer
    {
        public ModelScorer()
        {
            this.Stat = new ModelStat();
        }

        public ModelStat Stat { get; }

        public void Score(string expected, string obtained)
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
    }
}