using System.Linq;

namespace Grobid.NET
{
    public sealed class TeiFeatureMerge
    {
        private readonly TokenAligner<string> aligner;

        public TeiFeatureMerge()
        {
            this.aligner = new TokenAligner<string>();
        }

        public MergedFeatureRow[] Merge(TeiFeature[] teiFeatures, FeatureRow[] featureRows)
        {
            var aligned = this.aligner.Align(
                    teiFeatures,
                    x => x.Value,
                    featureRows,
                    x => x.Value,
                    (tei, row, value) => MergedFeatureRow.Create(value, tei.Classification, row.Classification))
                .ToArray();

            return aligned;
        }
    }
}