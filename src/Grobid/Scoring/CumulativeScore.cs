using System;
using System.Linq;

namespace Grobid.NET.Scoring
{
    public sealed class CumulativeScore
    {
        private CumulativeScore(LabelScore microLabelScore, LabelScore macroLabelScore)
        {
            this.MicroLabelScore = microLabelScore;
            this.MacroLabelScore = macroLabelScore;
        }

        public LabelScore MicroLabelScore { get; }
        public LabelScore MacroLabelScore { get; }

        public static CumulativeScore Create(LabelScore[] labelScores)
        {
            int cumulated_tp = 0;
            int cumulated_fp = 0;
            int cumulated_tn = 0;
            int cumulated_fn = 0;
            int cumulated_all = 0;

            double cumulated_accuracy = 0;
            double cumulated_precision = 0;
            double cumulated_recall = 0;
            double cumulated_f0 = 0;

            int totalFields = labelScores.Sum(x => x.Expected + x.FalsePositives);

            foreach (var labelStat in labelScores)
            {
                int tp = labelStat.TruePositives;
                int fp = labelStat.FalsePositives;
                int fn = labelStat.FalseNegatives;
                int tn = totalFields - tp - (fp + fn);
                int all = labelStat.Expected;

                double accuracy = (double)(tp + tn) / (tp + fp + tn + fn);
                double precision;
                if (tp + fp == 0)
                {
                    precision = 0.0;
                }
                else
                {
                    precision = (double)tp / (tp + fp);
                }

                double recall;
                if ((tp == 0) || (all == 0))
                {
                    recall = 0.0;
                }
                else
                {
                    recall = (double)tp / all;
                }

                double f0;
                if (Math.Abs(precision + recall) < 0.0000001)
                {
                    f0 = 0.0;
                }
                else
                {
                    f0 = 2 * precision * recall / (precision + recall);
                }

                cumulated_tp += tp;
                cumulated_fp += fp;
                cumulated_tn += tn;
                cumulated_fn += fn;

                if (all != 0)
                {
                    cumulated_all += all;
                    cumulated_f0 += f0;
                    cumulated_accuracy += accuracy;
                    cumulated_precision += precision;
                    cumulated_recall += recall;
                }
            }

            var cumulativeLabelStat = new LabelStat
            {
                Expected = cumulated_all,
                FalseNegative = cumulated_fn,
                FalsePositive = cumulated_fp,
                Observed = cumulated_tp,
            };

            var microLabelScore = new LabelScore("micro average", totalFields, cumulativeLabelStat);

            var totalValidFields = labelScores.Count(x => x.Expected > 0);
            var macroLabelScore = new LabelScore(
                "macro average",
                Math.Min(1.0, cumulated_accuracy / totalValidFields),
                Math.Min(1.0, cumulated_precision / totalValidFields),
                Math.Min(1.0, cumulated_recall / totalValidFields),
                Math.Min(1.0, cumulated_f0 / totalValidFields));

            return new CumulativeScore(microLabelScore, macroLabelScore);
        }
    }
}