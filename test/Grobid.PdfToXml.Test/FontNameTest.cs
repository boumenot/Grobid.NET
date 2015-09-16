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
            AssertionExtensions.Should((string)testSubject.Tag).Be("CHUFSU");
            AssertionExtensions.Should((string)testSubject.Name).Be("NimbusRomNo9L");
            AssertionExtensions.Should((string)testSubject.Weight).Be("Medi");
        }

        [Fact]
        public void ParseShouldHandleMissingFontWeight()
        {
            var testSubject = FontName.Parse("CHUFSU+NimbusRomNo9L");
            AssertionExtensions.Should((string)testSubject.Tag).Be("CHUFSU");
            AssertionExtensions.Should((string)testSubject.Name).Be("NimbusRomNo9L");
            AssertionExtensions.Should((string)testSubject.Weight).BeEmpty();
        }
    }
}
