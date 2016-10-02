using FluentAssertions;
using Xunit;

using Grobid.NET;


namespace Grobid.Test
{
    public class TeiFeatureFormatterTest
    {
        [Fact]
        public void TeiFeatureFormatter00()
        {
            var feature = new TeiFeature
            {
                Value = "The",
                Classification = "title",
                TokenIndex = 0,
            };

            var testSubject = new TeiFeatureFormatter();
            testSubject.Format(feature).Should().Be("The I-<title>");
            testSubject.Format(new [] { feature }).Should().HaveCount(1).And.Contain("The I-<title>");
        }

        [Fact]
        public void TeiFeatureFormatter01()
        {
            var feature = new TeiFeature
            {
                Value = "The",
                Classification = "title",
                TokenIndex = 1,
            };

            var testSubject = new TeiFeatureFormatter();
            testSubject.Format(feature).Should().Be("The <title>");
            testSubject.Format(new[] { feature }).Should().HaveCount(1).And.Contain("The <title>");
        }

        [Fact]
        public void TeiFeatureFormatter02()
        {
            var feature = new TeiFeature
            {
                Value = "@newline",
            };

            var testSubject = new TeiFeatureFormatter();
            testSubject.Format(feature).Should().Be("@newline");
            testSubject.Format(new[] { feature }).Should().HaveCount(1).And.Contain("@newline");
        }
    }
}
