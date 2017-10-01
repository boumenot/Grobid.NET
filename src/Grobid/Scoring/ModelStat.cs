using System.CodeDom;
using System.Collections.Generic;
using System.Linq;

namespace Grobid.NET.Scoring
{
    public sealed class ModelStat
    {
        private readonly Dictionary<string, LabelStat> labelStats;

        public ModelStat()
        {
            this.labelStats = new Dictionary<string, LabelStat>();
        }

        public string[] Labels => this.labelStats.Keys.ToArray();
        public IReadOnlyDictionary<string, LabelStat> LabelStats => this.labelStats;

        public LabelStat this[string s] => this.LabelStats[s];

        public void IncrExpected(string label)
        {
            var labelStat = this.Get(label);
            labelStat.Expected += 1;
        }

        public void IncrFalsePositive(string label)
        {
            var labelStat = this.Get(label);
            labelStat.FalsePositive += 1;
        }

        public void IncrFalseNegative(string label)
        {
            var labelStat = this.Get(label);
            labelStat.FalseNegative += 1;
        }

        public void IncrObserved(string label)
        {
            var labelStat = this.Get(label);
            labelStat.Observed+= 1;
        }

        private LabelStat Get(string label)
        {
            LabelStat labelStat;
            bool exists = this.labelStats.TryGetValue(label, out labelStat);
            if (exists)
            {
                return labelStat;
            }

            labelStat = new LabelStat();
            this.labelStats[label] = labelStat;

            return labelStat;
        }
    }
}