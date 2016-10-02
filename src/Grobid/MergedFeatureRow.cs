namespace Grobid.NET
{
    public struct MergedFeatureRow
    {
        public string Value { get; set; }
        public string Expected { get; set; }
        public string Actual { get; set; }

        public static MergedFeatureRow Create(string value, string expected, string actual)
        {
            var row = new MergedFeatureRow
            {
                Value = value,
                Expected = expected,
                Actual = actual,
            };

            return row;
        }
    }
}