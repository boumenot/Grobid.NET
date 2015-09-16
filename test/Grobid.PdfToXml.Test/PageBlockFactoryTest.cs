using FluentAssertions;
using Xunit;

namespace Grobid.PdfToXml.Test
{
    public class PageBlockFactoryTest
    {
        [Fact]
        public void Test()
        {
            var testInfoFactory = new PageBackFactory();
            var pageBlocks = testInfoFactory.Create(Sample.Pdf.OpenEssenseLinq(), 1);

            pageBlocks.Should().HaveCount(1);

            var tokenBlocks = pageBlocks[0].TokenBlocks;
            tokenBlocks.Should().HaveCount(1835);
        }
    }
}
