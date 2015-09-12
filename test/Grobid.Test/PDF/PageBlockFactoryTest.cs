using FluentAssertions;
using Grobid.NET;
using Xunit;

namespace Grobid.Test.PDF
{
    public class PageBlockFactoryTest
    {
        [Fact]
        public void Test()
        {
            var testInfoFactory = new PageBackFactory(1);
            var pageBlocks = testInfoFactory.Create(Sample.Pdf.OpenEssenseLinq());

            pageBlocks.Should().HaveCount(1);

            var tokenBlocks = pageBlocks[0].TokenBlocks;
            tokenBlocks.Should().HaveCount(1835);
        }
    }
}
