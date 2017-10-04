using Grobid.NET.Contract;

namespace Grobid.NET.Feature.Header
{
    public class HeaderFeatureFormatter : IFeatureVectorFormatter<HeaderFeatureVector>
    {
        public static readonly HeaderFeatureFormatter Instance = new HeaderFeatureFormatter();

        public string[] Format(HeaderFeatureVector feature)
        {
            return new[]
            {
                feature.Text,
                feature.AsLowerCase,
                feature.Prefix1,
                feature.Prefix2,
                feature.Prefix3,
                feature.Prefix4,
                feature.Suffix1,
                feature.Suffix2,
                feature.Suffix3,
                feature.Suffix4,
                feature.BlockStatus.ToString(),
                feature.LineStatus.ToString(),
                feature.FontStatus.ToString(),
                feature.FontSizeStatus.ToString(),
                feature.IsBold ? "1" : "0",
                feature.IsItalic ? "1" : "0",
                feature.IsRotation ? "1" : "0",
                feature.Capitalization.ToString(),
                feature.Digit.ToString(),
                feature.IsSingleChar ? "1" : "0",
                feature.IsProperName ? "1" : "0",
                feature.IsDictionaryWord ? "1" : "0",
                feature.IsFirstName ? "1" : "0",
                feature.IsLocationName ? "1" : "0",
                feature.IsEmailAddress ? "1" : "0",
                feature.IsYear ? "1" : "0",
                feature.IsMonth ? "1" : "0",
                feature.IsHttp ? "1" : "0",
                feature.HasDash ? "1" : "0",
                feature.Punctuation.ToString(),
                "0",
                "0",
            };
        }
    }
}
