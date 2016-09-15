namespace Grobid.NET
{
    public class HeaderFeatureVectorFactory
    {
        private readonly FeatureExtractor featureExtractor;

        public HeaderFeatureVectorFactory(FeatureExtractor featureExtractor)
        {
            this.featureExtractor = featureExtractor;
        }

        public HeaderFeatureVector Create(BlockState blockState)
        {
            var fv = new HeaderFeatureVector
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
                BlockStatus = blockState.BlockStatus,
                LineStatus = blockState.LineStatus,
                FontStatus = blockState.FontStatus,
                FontSizeStatus = blockState.FontSizeStatus,
                IsBold = blockState.IsBold,
                IsItalic = blockState.IsItalic,
                IsRotation = false, // TODO: add support for rotation (maybe)
                Capitalization = this.featureExtractor.Case(blockState.Text),
                Digit = this.featureExtractor.Digit(blockState.Text),
                IsSingleChar = this.featureExtractor.IsSingleChar(blockState.Text),
                IsProperName = this.featureExtractor.IsName(blockState.Text),
                IsDictionaryWord = this.featureExtractor.IsDictionaryWord(blockState.Text),
                //IsFirstName = this.featureExtractor.IsForename(blockState.Text),
                IsFirstName = false, // XXX: why is this not used?
                IsLocationName = this.featureExtractor.IsLocationName(blockState.Text),
                IsYear = this.featureExtractor.IsYear(blockState.Text),
                IsMonth = this.featureExtractor.IsMonth(blockState.Text),
                IsEmailAddress = this.featureExtractor.IsEmailAddress(blockState.Text),
                IsHttp = this.featureExtractor.HasHttp(blockState.Text),
                HasDash = this.featureExtractor.HasDash(blockState.Text),
                Punctuation = this.featureExtractor.Punctuation(blockState.Text),
            };

            return fv;
        }
    }
}