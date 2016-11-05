using System;

namespace Grobid.NET
{
    public class LabelScore
    {
        private const double Epsilon = 0.0000001;

        private readonly int total;
        private readonly LabelStat labelStat;

        public LabelScore(string label, int total, LabelStat labelStat)
        {
            this.Label = label;
            this.total = total;
            this.labelStat = labelStat;

            this.Accuracy = LabelScore.CalculateAccuracy(
                this.TruePositives,
                this.TrueNegatives,
                this.FalsePositives,
                this.FalseNegatives);

            this.Precision = LabelScore.CalculatePrecision(
                this.TruePositives,
                this.FalsePositives);

            this.Recall = LabelScore.CalcaulateRecall(
                this.TruePositives,
                this.labelStat.Expected);

            this.F0 = LabelScore.CalculateF0(this.Precision, this.Recall);
        }

        public LabelScore(string label, double accuracy, double precision, double recall, double f0)
        {
            this.Label = label;
            this.Accuracy = accuracy;
            this.Precision = precision;
            this.Recall = recall;
            this.F0 = f0;
        }

        public string Label { get; }
        public double Accuracy { get; }
        public double Precision { get; }
        public double Recall { get; }
        public double F0 { get; }

        internal int TruePositives => this.labelStat.Observed;
        internal int TrueNegatives => this.total - this.TruePositives - (this.FalsePositives + this.FalseNegatives);
        internal int FalsePositives => this.labelStat.FalsePositive;
        internal int FalseNegatives => this.labelStat.FalseNegative;
        internal int Expected => this.labelStat.Expected;

        internal static double CalculateF0(double precision, double recall)
        {
            if (Math.Abs(precision + recall) < LabelScore.Epsilon)
            {
                return 0.0;
            }

            return (2 * precision * recall) / (precision + recall);
        }

        internal static double CalcaulateRecall(int truePositives, int all)
        {
            if (truePositives == 0 || all == 0)
            {
                return 0.0;
            }

            return (double)truePositives / all;
        }

        internal static double CalculatePrecision(int truePositives, int falsePositives)
        {
            if (truePositives + falsePositives == 0)
            {
                return 0.0;
            }

            return (double)truePositives / (truePositives + falsePositives);
        }

        internal static double CalculateAccuracy(
            int truePositives,
            int trueNegatives,
            int falsePositives,
            int falseNegatives)
        {
            return (double)(truePositives + trueNegatives) / (truePositives + falsePositives + trueNegatives + falseNegatives);
        }
    }
}