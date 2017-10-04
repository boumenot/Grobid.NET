using Grobid.NET.Feature.Date;

namespace Grobid.NET
{
    public class LabeledFeature
    {
        public string Text => this.FeatureVector.Text;
        public string Classification { get; set; }
        public int TokenIndex { get; set; }
        public bool IsStart => this.TokenIndex == 0;
        public DateFeatureVector FeatureVector { get; set; }

        public static LabeledFeature Create(DateFeatureVector featureVector, string classification, int tokenIndex)
        {
            var labeledFeature = new LabeledFeature
            {
                FeatureVector = featureVector,
                Classification = classification,
                TokenIndex = tokenIndex,
            };

            return labeledFeature;
        }
    }
}
