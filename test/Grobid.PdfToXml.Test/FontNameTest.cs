using FluentAssertions;

using Xunit;

namespace Grobid.PdfToXml.Test
{
    public class FontNameTest
    {
        [Fact]
        public void ParseShouldReturnFontName()
        {
            var testSubject = FontName.Parse("CHUFSU+NimbusRomNo9L-Medi");
            testSubject.Tag.Should().Be("CHUFSU");
            testSubject.Name.Should().Be("NimbusRomNo9L");
            testSubject.FullName.Should().Be("CHUFSU+NimbusRomNo9L-Medi");
        }

        [Fact]
        public void ParseShouldHandleMissingFontWeight()
        {
            var testSubject = FontName.Parse("CHUFSU+NimbusRomNo9L");
            testSubject.Tag.Should().Be("CHUFSU");
            testSubject.Name.Should().Be("NimbusRomNo9L");
            testSubject.Weight.Should().BeEmpty();
            testSubject.FullName.Should().Be("CHUFSU+NimbusRomNo9L");
        }
    }
}
