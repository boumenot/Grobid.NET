namespace Grobid.NET.Feature.Date
{
    public class DateFeatureFormatter
    {
        public static readonly DateFeatureFormatter Instance = new DateFeatureFormatter();

        public string[] Format(DateFeatureVector feature)
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
                feature.LineStatus.ToString(),
                feature.Capitalization.ToString(),
                feature.Digit.ToString(),
                feature.IsSingleChar ? "1" : "0",
                feature.IsYear ? "1" : "0",
                feature.IsMonth ? "1" : "0",
                feature.Punctuation.ToString(),
            };
        }
    }
}
