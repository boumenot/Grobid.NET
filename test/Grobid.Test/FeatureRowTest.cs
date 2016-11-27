using System;

using FluentAssertions;
using Xunit;

using Grobid.NET;

namespace Grobid.Test
{
    public class FeatureRowTest
    {
        [Fact]
        public void FeatureRow00()
        {
            var testSubject = FeatureRow.Parse("The the 1 1 0 1 0 LINESTART 0 1 I-<title>");

            testSubject.Classification.Should().Be("title");
            testSubject.Value.Should().Be("The");
            testSubject.IsStart.Should().BeTrue();
        }

        [Fact]
        public void FeatureRow01()
        {
            var testSubject = FeatureRow.Parse("The the 1 1 0 1 0 LINESTART 0 1 <title>");

            testSubject.Classification.Should().Be("title");
            testSubject.Value.Should().Be("The");
            testSubject.IsStart.Should().BeFalse();
        }

        [Fact]
        public void FeatureRow02()
        {
            var testSubject = FeatureRow.Parse("The the 1 1 0 1 0 LINESTART 0 1 <do-not-select> <title>");

            testSubject.Classification.Should().Be("title");
            testSubject.Value.Should().Be("The");
            testSubject.IsStart.Should().BeFalse();
        }

        [Fact]
        public void FeatureRow03()
        {
            var testSubject = FeatureRow.Parse("The the 1 1 0 1 0 LINESTART 0 1");

            testSubject.Classification.Should().BeEmpty();
            testSubject.Value.Should().Be("The");
            testSubject.IsStart.Should().BeFalse();
        }

        [Fact]
        public void FeatureRow04()
        {
            var malformedFeatureRows = new[]
            {
                "The the 1 1 0 1 0 LINESTART 0 1 <title",
                //"The the 1 1 0 1 0 LINESTART 0 1 title>",
                "The the 1 1 0 1 0 LINESTART 0 1 >title<",
                "The\tthe\t1\t1\t0\t1\t0\tLINESTART\t0\t1\t<title>",
                "<title>",
            };

            foreach (var malformedFeatureRow in malformedFeatureRows)
            {
                Action action = () => FeatureRow.Parse(malformedFeatureRow);
                action.ShouldThrow<ArgumentException>(malformedFeatureRow);
            }
        }
    }
}
