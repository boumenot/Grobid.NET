namespace Grobid.NET.Feature.Date
{
    public class DateFeatureVectorFactory
    {
        private readonly FeatureExtractor featureExtractor;

        public DateFeatureVectorFactory(FeatureExtractor featureExtractor)
        {
            this.featureExtractor = featureExtractor;
        }

        //November november N No Nov Nove r er ber mber LINESTART INITCAP NODIGIT 0 0 1 NOPUNCT I-<month>
        public DateFeatureVector Create(BlockState blockState)
        {
            var fv = new DateFeatureVector
            {
                Text = blockState.Text,
                AsLowerCase = blockState.Text.ToLower(),
                Prefix1 = this.featureExtractor.Prefix(blockState.Text, 1),
                Prefix2 = this.featureExtractor.Prefix(blockState.Text, 2),
                Prefix3 = this.featureExtractor.Prefix(blockState.Text, 3),
                Prefix4 = this.featureExtractor.Prefix(blockState.Text, 4),
                Suffix1 = this.featureExtractor.Suffix(blockState.Text, 1),
                Suffix2 = this.featureExtractor.Suffix(blockState.Text, 2),
                Suffix3 = this.featureExtractor.Suffix(blockState.Text, 3),
                Suffix4 = this.featureExtractor.Suffix(blockState.Text, 4),
                LineStatus = blockState.LineStatus,
                Capitalization = this.featureExtractor.Case(blockState.Text),
                Digit = this.featureExtractor.Digit(blockState.Text),
                IsSingleChar = this.featureExtractor.IsSingleChar(blockState.Text),
                IsYear = this.featureExtractor.IsYear(blockState.Text),
                IsMonth = this.featureExtractor.IsMonth(blockState.Text),
                Punctuation = this.featureExtractor.Punctuation(blockState.Text),
            };

            return fv;
        }
    }
}
