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
        public void IsBoldShouldBeTrue()
        {
            var testSubject = FontName.Parse("CHUFSU+NimbusRomNo9L-Bold");
            testSubject.IsBold.Should().BeTrue();
        }

        [Fact]
        public void IsBoldShouldBeFalse()
        {
            var testSubject = FontName.Parse("CHUFSU+NimbusRomNo9L-Medi");
            testSubject.IsBold.Should().BeFalse();
        }

        [Fact]
        public void IsItalicForObliqueShouldBeTrue()
        {
            var testSubject = FontName.Parse("CHUFSU+NimbusRomNo9L-Oblique");
            testSubject.IsItalic.Should().BeTrue();
        }

        [Fact]
        public void IsItalicForItalicShouldBeTrue()
        {
            var testSubject = FontName.Parse("CHUFSU+NimbusRomNo9L-Italic");
            testSubject.IsItalic.Should().BeTrue();
        }

        [Fact]
        public void IsItalicShouldBeFalse()
        {
            var testSubject = FontName.Parse("CHUFSU+NimbusRomNo9L-Medi");
            testSubject.IsItalic.Should().BeFalse();
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
