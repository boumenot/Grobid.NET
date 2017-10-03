using Grobid.NET.Feature;

namespace Grobid.NET
{
    public class DateFeatureVector
    {
        public string Text { get; set; }
        public string AsLowerCase { get; set; }
        public string Prefix1 { get; set; }
        public string Prefix2 { get; set; }
        public string Prefix3 { get; set; }
        public string Prefix4 { get; set; }
        public string Suffix1 { get; set; }
        public string Suffix2 { get; set; }
        public string Suffix3 { get; set; }
        public string Suffix4 { get; set; }
        public LineStatus LineStatus { get; set; }
        public Capitalization Capitalization { get; set; }
        public Digit Digit { get; set; }
        public bool IsSingleChar { get; set; }
        public bool IsYear { get; set; }
        public bool IsMonth { get; set; }
        public Punctuation Punctuation { get; set; }
    }
}
