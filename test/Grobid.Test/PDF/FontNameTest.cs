using FluentAssertions;
using Grobid.NET;
using Xunit;

namespace Grobid.Test.PDF
{
    public class FontNameTest
    {
        [Fact]
        public void ParseShouldReturnFontName()
        {
            var testSubject = FontName.Parse("CHUFSU+NimbusRomNo9L-Medi");
            testSubject.Tag.Should().Be("CHUFSU");
            testSubject.Name.Should().Be("NimbusRomNo9L");
            testSubject.Weight.Should().Be("Medi");
        }
    }
}
