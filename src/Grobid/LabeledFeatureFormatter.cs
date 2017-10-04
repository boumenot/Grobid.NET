using System.Linq;
using Grobid.NET.Contract;

namespace Grobid.NET
{
    public class LabeledFeatureFormatter<T>
    {
        private readonly IFeatureVectorFormatter<T> formatter;

        public LabeledFeatureFormatter(IFeatureVectorFormatter<T> formatter)
        {
            this.formatter = formatter;
        }

        public string[] Format(T featureVector, string classification)
        {
            return this.formatter.Format(featureVector).Concat(new[] { classification }).ToArray();
        }

        public string[] Format(T featureVector, LabeledFeature labeledFeature)
        {
            return this.formatter.Format(featureVector).Concat(new[] { this.GetClassification(labeledFeature) }).ToArray();
        }

        private string GetClassification(LabeledFeature labeledFeature)
        {
            return labeledFeature.IsStart ?
                $"I-<{labeledFeature.Classification}>" :
                $"<{labeledFeature.Classification}>";
        }
    }
}
