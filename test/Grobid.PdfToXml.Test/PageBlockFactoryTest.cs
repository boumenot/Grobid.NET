using System.Linq;

using FluentAssertions;
using Xunit;

namespace Grobid.PdfToXml.Test
{
    public class PageBlockFactoryTest
    {
        [Fact]
        public void Test()
        {
            var testInfoFactory = new PageBlockFactory();
            var pageBlocks = testInfoFactory.Create(Sample.Pdf.OpenEssenseLinq(), 1);

            pageBlocks.Should().HaveCount(1);
            pageBlocks[0].Offset.Should().Be(1);

            var textBlocks = pageBlocks[0].Blocks.SelectMany(x => x.TextBlocks);
            textBlocks.Should().HaveCount(104);
        }
    }
}
