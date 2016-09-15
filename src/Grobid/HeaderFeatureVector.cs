namespace Grobid.NET
{
    public class HeaderFeatureVector
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
        public BlockStatus BlockStatus { get; set; }
        public LineStatus LineStatus { get; set; }
        public FontStatus FontStatus { get; set; }
        public FontSizeStatus FontSizeStatus { get; set; }
        public bool IsBold { get; set; }
        public bool IsItalic { get; set; }
        public bool IsRotation { get; set; }
        public Capitalization Capitalization { get; set; }
        public Digit Digit { get; set; }
        public bool IsSingleChar { get; set; }
        public bool IsProperName { get; set; }
        public bool IsDictionaryWord { get; set; }
        public bool IsFirstName { get; set; }
        public bool IsLocationName { get; set; }
        public bool IsYear { get; set; }
        public bool IsMonth { get; set; }
        public bool IsEmailAddress { get; set; }
        public bool IsHttp { get; set; }
        public bool HasDash { get; set; }
        public Punctuation Punctuation { get; set; }
    }
}